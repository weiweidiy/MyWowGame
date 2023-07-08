using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ConfigTools
{
    public class CodeHelper
    {
        public static void GenCode(TableMeta pTableMeta, string pPath, ExportCfgType pCfgType)
        {
            if (pCfgType == ExportCfgType.Client)
                GenCSCode(pTableMeta, pPath, pCfgType);
            else if (pCfgType == ExportCfgType.Server)
                GenGOCode(pTableMeta, pPath, pCfgType);
        }

        private static void GenCSCode(TableMeta pTableMeta, string pPath, ExportCfgType pCfgType)
        {
            var _Path = Path.Combine(pPath, pTableMeta.ClassName + ".cs");
            using (var fs = new FileStream(_Path, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    if (!pTableMeta.CheckTypeIsMap())
                    {
                        MessageBox.Show($"表 {pTableMeta.ClassName} 的第一列不是ID字段");
                        return;
                    }
                    sw.WriteLine("/*");
                    sw.WriteLine("* 此类由ConfigTools自动生成. 不要手动修改!");
                    sw.WriteLine("*/");
                    sw.WriteLine("using System.Collections.Generic;");
                    sw.WriteLine("using Logic.Config;");
                    sw.WriteLine("");
                    sw.WriteLine("namespace Configs");
                    sw.WriteLine("{");
                    sw.WriteLine("    public partial class {0}", pTableMeta.ClassName);
                    sw.WriteLine("    {");
                    sw.WriteLine("        public Dictionary<string, {0}> AllData;", pTableMeta.DataName);
                    sw.WriteLine("        public static {0} GetData(int pID)", pTableMeta.DataName);
                    sw.WriteLine("        {");
                    sw.WriteLine("            return ConfigManager.Ins.m_{0}.AllData.TryGetValue(pID.ToString(), out var _data) ? _data : null;", pTableMeta.ClassName);
                    sw.WriteLine("        }");
                    sw.WriteLine("    }");
                    sw.WriteLine();
                    sw.WriteLine("    public class {0}", pTableMeta.DataName);
                    sw.WriteLine("    {");
                    foreach (var field in pTableMeta.Fields)
                    {
                        if (!field.IsExportField(pCfgType))
                            continue;

                        if (!string.IsNullOrWhiteSpace(field.mSpostil))
                        {
                            sw.WriteLine("        /*");
                            sw.WriteLine("        " + field.mSpostil);
                            sw.WriteLine("        */");
                        }

                        sw.WriteLine("        " + field.mComment);
                        sw.WriteLine("        public {0} {1};", field.GetFieldTypeName(ExportCfgType.Client),
                            field.mFieldName);
                        sw.WriteLine();
                    }
                    sw.WriteLine("    }");
                    sw.WriteLine("}");
                }
            }
        }

        private static void GenGOCode(TableMeta pTableMeta, string pPath, ExportCfgType pCfgType)
        {
            var _Path = Path.Combine(pPath, pTableMeta.ClassName + ".go");
            using (var fs = new FileStream(_Path, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine("/*");
                    sw.WriteLine("* 此类由ConfigTools自动生成. 不要手动修改!");
                    sw.WriteLine("*/");
                    sw.WriteLine();
                    sw.WriteLine("package Configs");
                    sw.WriteLine();
                    sw.WriteLine("import (");
                    sw.WriteLine("    \"encoding/json\"");
                    sw.WriteLine("    \"fmt\"");
                    sw.WriteLine("    \"os\"");
                    sw.WriteLine(")");
                    sw.WriteLine();
                    sw.WriteLine("var {0} {1}", pTableMeta.ClassName + "Data", pTableMeta.ClassName);
                    sw.WriteLine();
                    
                    sw.WriteLine("type {0} struct {{", pTableMeta.ClassName);
                    sw.WriteLine("    AllData map[int32]*{0}", pTableMeta.DataName);
                    sw.WriteLine("}");
                    sw.WriteLine();
                    
                    sw.WriteLine("type {0} struct {{", pTableMeta.DataName);
                    foreach (var field in pTableMeta.Fields)
                    {
                        if (!field.IsExportField(pCfgType))
                            continue;

                        if (!string.IsNullOrWhiteSpace(field.mSpostil))
                        {
                            sw.WriteLine("        /*");
                            sw.WriteLine("        " + field.mSpostil);
                            sw.WriteLine("        */");
                        }

                        sw.WriteLine("    {0} {1}  {2}", field.mFieldName, field.GetFieldTypeName(ExportCfgType.Server), field.mComment);
                    }
                    sw.WriteLine("}");
                    sw.WriteLine();
                    
                    sw.WriteLine("func Load{0}(pDir string) {{", pTableMeta.ClassName);
                    sw.WriteLine("    file, err := os.ReadFile(pDir + `/{0}.json`)", pTableMeta.ClassName);
                    sw.WriteLine("    if err != nil {");
                    sw.WriteLine("        panic(fmt.Sprintf(\"Load [{0}] Failed. {{%s}}\", err))", pTableMeta.ClassName);
                    sw.WriteLine("    }");
                    sw.WriteLine();
                    sw.WriteLine("    err = json.Unmarshal(file, &{0})",pTableMeta.ClassName + "Data");
                    sw.WriteLine("    if err != nil {");
                    sw.WriteLine("        panic(fmt.Sprintf(\"Unmarshal [{0}] Failed. {{%s}}\", err))", pTableMeta.ClassName);
                    sw.WriteLine("        return");
                    sw.WriteLine("    }");
                    sw.WriteLine("}");
                    sw.WriteLine();
                    
                    sw.WriteLine("func Get{0}(id int32) *{0} {{", pTableMeta.DataName);
                    sw.WriteLine("    data, have := {0}.AllData[id]", pTableMeta.ClassName + "Data");
                    sw.WriteLine("    if have {");
                    sw.WriteLine("        return data");
                    sw.WriteLine("    }");
                    sw.WriteLine("    return nil");
                    sw.WriteLine("}");
                }
            }
        }
    }
}