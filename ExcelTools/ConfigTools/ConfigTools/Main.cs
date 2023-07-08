using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConfigTools
{
    public partial class Main : Form
    {
        //Excel路径
        public static string sExcelPath = "";
        //代码路径
        public static string sClientCodePath = "";
        //配置路径
        public static string sClientCfgPath = "";
        //代码路径
        public static string sServerCodePath = "";
        //配置路径
        public static string sServerCfgPath = "";
        //导出配置类型
        public static ExportCfgType sExportCfgType = ExportCfgType.Client;
        //是否导出代码
        public static bool sCanExportCode = true;
        //是否导出配置
        public static bool sCanExportCfg = true;
        //是否压缩配置
        public static bool sNeedCom = false;

        public static List<ExcelFileInfo> sExcelFileList = new List<ExcelFileInfo>(64);

        public Main()
        {
            InitializeComponent();
        }

        //初始化
        private void Main_Load(object sender, EventArgs e)
        {
            RegistryHelper.InitData();
            mExcelPath.Text = sExcelPath;
            

            if (sExportCfgType == ExportCfgType.Client)
            {
                mCodeOutPath.Text = sClientCodePath;
                mCfgOutPath.Text = sClientCfgPath;

                rbServer.Checked = false;
                rbClient.Checked = true;
            }
            else
            {
                mCodeOutPath.Text = sServerCodePath;
                mCfgOutPath.Text = sServerCfgPath;

                rbServer.Checked = true;
                rbClient.Checked = false;
            }

            cbGenCode.Checked = sCanExportCode;
            cbGenCfg.Checked = sCanExportCfg;

            RefreshExcels();
        }

        //选择Excel文件目录
        private void btnExcelPath_Click(object sender, EventArgs e)
        {
            var _fbd = new FolderBrowserDialog {Description = "选择Excel文件目录"};
            if (_fbd.ShowDialog() == DialogResult.OK)
            {
                sExcelPath = _fbd.SelectedPath;
                mExcelPath.Text = sExcelPath;
                RefreshExcels();
            }
        }

        //选择代码目录
        private void btnCodePath_Click(object sender, EventArgs e)
        {
            var _fbd = new FolderBrowserDialog {Description = "选择代码目录"};
            if (_fbd.ShowDialog() == DialogResult.OK)
            {
                if (sExportCfgType == ExportCfgType.Client)
                    sClientCodePath = _fbd.SelectedPath;
                else
                    sServerCodePath = _fbd.SelectedPath;

                mCodeOutPath.Text = _fbd.SelectedPath;
            }
        }

        //选择配置表目录
        private void btnCfgPath_Click(object sender, EventArgs e)
        {
            var _fbd = new FolderBrowserDialog {Description = "选择配置表目录"};
            if (_fbd.ShowDialog() == DialogResult.OK)
            {
                if (sExportCfgType == ExportCfgType.Client)
                    sClientCfgPath = _fbd.SelectedPath;
                else
                    sServerCfgPath = _fbd.SelectedPath;

                mCfgOutPath.Text = _fbd.SelectedPath;
            }
        }

        //刷新配置表
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshExcels();
            AddLog("", false);
        }

        //是否生成代码
        private void cbGenCode_CheckedChanged(object sender, EventArgs e)
        {
            sCanExportCode = cbGenCode.Checked;
        }

        //是否生成配置
        private void cbGenCfg_CheckedChanged(object sender, EventArgs e)
        {
            sCanExportCfg = cbGenCfg.Checked;
        }

        //生成客户端配置
        private void rbClient_CheckedChanged(object sender, EventArgs e)
        {
            sExportCfgType = ExportCfgType.Client;
            mCodeOutPath.Text = sClientCodePath;
            mCfgOutPath.Text = sClientCfgPath;
            RegistryHelper.SaveData();
        }

        //生成服务器配置
        private void rbServer_CheckedChanged(object sender, EventArgs e)
        {
            sExportCfgType = ExportCfgType.Server;
            mCodeOutPath.Text = sServerCodePath;
            mCfgOutPath.Text = sServerCfgPath;
            RegistryHelper.SaveData();
        }

        //全部选择
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clbCfgFiles.Items.Count; i++)
                clbCfgFiles.SetItemChecked(i, true);
        }

        //全部取消
        private void btnAllUncheck_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clbCfgFiles.Items.Count; i++)
                clbCfgFiles.SetItemChecked(i, false);
        }

        //刷新Excel列表
        private void RefreshExcels()
        {
            clbCfgFiles.Items.Clear();
            var _Path = mExcelPath.Text;
            FileHelper.GetExcelFiles(_Path, sExcelFileList);
            foreach (var excelFileInfo in sExcelFileList)
                clbCfgFiles.Items.Add(new ExcelFileInfo
                {
                    Name = excelFileInfo.Name,
                    Path = excelFileInfo.Path
                }, CheckState.Unchecked);

            RefreshAllPaths();
        }

        //刷新目录
        private void RefreshAllPaths()
        {
            sExcelPath = mExcelPath.Text;
            if (sExportCfgType == ExportCfgType.Client)
            {
                sClientCfgPath = mCfgOutPath.Text;
                sClientCodePath = mCodeOutPath.Text;
            }
            else
            {
                sServerCfgPath = mCfgOutPath.Text;
                sServerCodePath = mCodeOutPath.Text;
            }
        }

        //生成按钮
        private void btnGen_Click(object sender, EventArgs e)
        {
            RefreshAllPaths();
            ShowInfo();

            for (var i = 0; i < clbCfgFiles.Items.Count; i++)
            {
                if (clbCfgFiles.GetItemCheckState(i) != CheckState.Checked)
                    continue;

                var excelFileInfo = clbCfgFiles.Items[i] as ExcelFileInfo;

                var dt = ExcelHelper.ImportExcelFile(excelFileInfo.Path);
                var meta = ExcelHelper.ParseTableMeta(excelFileInfo.Name, dt, ExportCfgType.Client);

                //生成代码
                if (sCanExportCode)
                    try
                    {
                        AddLog($"开始生成[ {meta.TableName} ]代码");
                        CodeHelper.GenCode(meta, sExportCfgType == ExportCfgType.Client ? sClientCodePath : sServerCodePath, sExportCfgType);
                        AddLog($"生成[ {meta.TableName} ]代码成功");
                    }
                    catch (Exception exp)
                    {
                        AddLog($"生成[{meta.TableName}]代码出现异常 => {exp.Message}");
                    }

                //生成配置
                if (sCanExportCfg)
                    try
                    {
                        AddLog($"开始生成[ {meta.TableName} ]配置");
                        CfgHelper.GenCfg(dt, sExportCfgType == ExportCfgType.Client ? sClientCfgPath : sServerCfgPath, meta, sExportCfgType);
                        AddLog($"生成[ {meta.TableName} ]配置成功");
                    }
                    catch (Exception exp)
                    {
                        AddLog($"生成[{meta.TableName}]配置出现异常 => {exp.Message}");
                        return;
                    }

                AddLog("");
            }
            RegistryHelper.SaveData();
        }

        public void ShowInfo()
        {
            AddLog("", false);
            if (sExportCfgType == ExportCfgType.Client)
            {
                AddLog($"配置输入路径: [{sClientCfgPath}]");
                AddLog($"代码输出路径: [{sClientCodePath}]");
            }
            else
            {
                AddLog($"配置输入路径: [{sServerCfgPath}]");
                AddLog($"代码输出路径: [{sServerCodePath}]");
            }

            AddLog($"是否导出配置 [{sCanExportCfg}]");
            AddLog($"配置类型 [{sExportCfgType}]");
            AddLog($"是否导出代码 [{sCanExportCode}]");
            AddLog("");
        }

        public void AddLog(string pLog, bool pAppend = true)
        {
            if (pAppend)
                textLog.AppendText(pLog + "\r\n");
            else
                textLog.Text = pLog;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (NeedCom.Checked)
            {
                sNeedCom = true;
            }
            else
            {
                sNeedCom = false;
            }
        }
    }
}