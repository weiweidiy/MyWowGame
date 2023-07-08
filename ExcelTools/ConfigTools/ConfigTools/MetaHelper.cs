using System.Collections.Generic;

namespace ConfigTools
{
    public class TableField
    {
        public static string[] GO_TypeNames =
        {
            "int32", "float32", "string", "bool", "[]int32", "[]float32",
            "[]string"
        };

        public static string[] CSharp_TypeNames =
        {
            "int", "float", "string", "bool", "List<int>", "List<float>",
            "List<string>"
        };

        public string mSpostil;
        public string mComment;
        public string mExportType;

        public string mFieldName;
        public string mTypeName;

        public string GetFieldTypeName(ExportCfgType pType)
        {
            var _type = TableFieldType.TFT_Int;
            if (mTypeName == "int")
                _type = TableFieldType.TFT_Int;
            else if (mTypeName == "float")
                _type = TableFieldType.TFT_Float;
            else if (mTypeName == "string")
                _type = TableFieldType.TFT_String;
            else if (mTypeName == "bool")
                _type = TableFieldType.TFT_Bool;
            else if (mTypeName == "int+")
                _type = TableFieldType.TFT_IntList;
            else if (mTypeName == "float+")
                _type = TableFieldType.TFT_FloatList;
            else if (mTypeName == "string+")
                _type = TableFieldType.TFT_StringList;


            if (pType == ExportCfgType.Client)
                return CSharp_TypeNames[(int) _type];
            if (pType == ExportCfgType.Server)
                return GO_TypeNames[(int) _type];

            return null;
        }

        public bool IsExportField(ExportCfgType pExpType)
        {
            if (mExportType == "all")
                return true;

            if ((pExpType == ExportCfgType.Client) && (mExportType == "client"))
                return true;

            if ((pExpType == ExportCfgType.Server) && (mExportType == "server"))
                return true;

            return false;
        }
    }

    public class TableMeta
    {
        public List<TableField> Fields = new List<TableField>(8);
        public string TableName;

        public string ClassName => TableName + "Cfg";
        public string DataName => TableName + "Data";

        public bool CheckTypeIsMap()
        {
            return Fields[0].mFieldName.ToUpper() == "ID";
        }
    }
}