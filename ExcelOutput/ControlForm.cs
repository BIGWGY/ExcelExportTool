using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ikvm.extensions;
using Microsoft.Extensions.Logging;

namespace ExcelTool
{
    public partial class ControlForm : Form
    {
        private ExcelContext _excelContext = new ExcelContext();

        private const string IniSectionName = "ExcelTool";


        private const string IniNameOfEnumDirectory = "EnumDirectory";
        private const string IniNameOfCsCodeNamespace = "CsCodeNamespace";
        private const string IniNameOfExcelDataDirectory = "ExcelDataDirectory";
        private const string IniNameOfJsonOutputDataDirectory = "JsonOutputDataDirectory";
        private const string IniNameOfBinaryOutputDataDirectory = "BinaryOutputDataDirectory";
        private const string IniNameOfCsEnumCodeOutputDirectory = "CsEnumCodeOutputDirectory";
        private const string IniNameOfCsTableCodeOutputDirectory = "CsTableCodeOutputDirectory";
        
        private IniFile _iniFile = new IniFile(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "tool.ini");
        
        public ControlForm()
        {
            InitializeComponent();
            InitConfig();
            _excelContext.logMsgHandler += WriteLog;
        }

        /// <summary>
        /// 设置枚举目录
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        private void SetEnumDirectory(string value)
        {
            if (value.Equals(""))
            {
                value = Environment.CurrentDirectory;
            }

            EnumLabel.Text = value;
            _excelContext.EnumDirectory = value;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfEnumDirectory, value);
        }
        
        /// <summary>
        /// 设置导出的cs代码的命名空间.
        /// </summary>
        /// <param name="value"></param>
        private void SetCsCodeNamespace(string value, string defaultValue = "TestCode")
        {
            if (value.Equals(""))
            {
                value = defaultValue;
            }
            CsNamespaceEdit.Text = value;
            _excelContext.ChsarpCodeNameSpace = value;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfCsCodeNamespace, value);   
        }
        
        /// <summary>
        /// 设置excel路径。
        /// </summary>
        /// <param name="directory"></param>
        private void SetExcelDataDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }
            
            ExcelPathText.Text = directory;
            TableCheckBoxList.Items.Clear();
            _excelContext.ExcelDataDirectory = directory;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfExcelDataDirectory, directory);

            string filterWord = ExcelFilterTextBox.Text;
            List<FileInfo> fileInfos = ExcelContext.FilterFile(directory, ExcelContext.ExcelSuffix);
            
            foreach (var fileInfo in fileInfos)
            {
                if (filterWord.Equals(""))
                {
                    TableCheckBoxList.Items.Add(fileInfo);    
                }
                else if(fileInfo.Name.Contains(filterWord))
                {
                    TableCheckBoxList.Items.Add(fileInfo);
                }
            }
        }

        /// <summary>
        /// 设置二进制数据导出目录。
        /// </summary>
        /// <param name="directory"></param>
        private void SetBinaryOutputDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            BinaryOutPath.Text = directory;
            _excelContext.ClientDataOutDirectory = directory;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfBinaryOutputDataDirectory, directory);
        }

        /// <summary>
        /// 设置json文件的输出目录。
        /// </summary>
        /// <param name="directory"></param>
        public void SetJsonOutputDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            JsonOutPath.Text = directory;
            _excelContext.ServerDataOutDirectory = directory;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfJsonOutputDataDirectory, directory);
        }

        /// <summary>
        /// 设置导出枚举目录。
        /// </summary>
        /// <param name="directory"></param>
        public void SetCsEnumCodeOutputDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            CsEnumCodeOutputPath.Text = directory;
            _excelContext.CsharpEnumCodeOutDirectory = directory;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfCsEnumCodeOutputDirectory, directory);
        }

        /// <summary>
        /// 设置导出cs代码的目录。
        /// </summary>
        /// <param name="directory"></param>
        public void SetCsTableCodeOutputDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            CsTableCodeOutputPath.Text = directory;
            _excelContext.CsharpTableCodeOutDirectory = directory;
            _iniFile.IniWriteValue(IniSectionName, IniNameOfCsTableCodeOutputDirectory, directory);
        }
        
        /// <summary>
        /// 返回选中的excel文件。
        /// </summary>
        /// <returns></returns>
        private List<FileInfo> GetCurrentSelectFileInfos()
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            for (int i = 0; i < TableCheckBoxList.Items.Count; i++)
            {
                if (TableCheckBoxList.GetItemChecked(i))
                {
                    fileInfos.Add((FileInfo)TableCheckBoxList.Items[i]);
                }
            }

            return fileInfos;
        }
        
        private void InitConfig()
        {
            SetEnumDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfEnumDirectory, _excelContext.EnumDirectory));
            SetCsCodeNamespace(_iniFile.IniReadValue(IniSectionName, IniNameOfCsCodeNamespace, _excelContext.ChsarpCodeNameSpace));
            SetExcelDataDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfExcelDataDirectory, _excelContext.ExcelDataDirectory));
            SetBinaryOutputDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfBinaryOutputDataDirectory, _excelContext.ClientDataOutDirectory));
            SetJsonOutputDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfJsonOutputDataDirectory, _excelContext.ServerDataOutDirectory));
            SetCsEnumCodeOutputDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfCsEnumCodeOutputDirectory, _excelContext.CsharpEnumCodeOutDirectory));
            SetCsTableCodeOutputDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfCsTableCodeOutputDirectory, _excelContext.CsharpTableCodeOutDirectory));
        }

        /// <summary>
        /// 选择文件夹。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        private string SelectFolder(string name, string defaultPath)
        {
            if (defaultPath.Equals("") || !Directory.Exists(defaultPath))
            {
                defaultPath = Environment.CurrentDirectory;
            }
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Path.GetFullPath(defaultPath);
            SendKeys.Send("{TAB}{TAB}{RIGHT}");
            dialog.Description = name;
            
            string path ="";
            
            var ret = dialog.ShowDialog();
            if (ret == DialogResult.OK || ret == DialogResult.Yes)
            {
                path = dialog.SelectedPath;
            }
            else
            {
                path = defaultPath;
            }
            
            return path;
        }


        /// <summary>
        /// 选择数据表文件夹。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectExcelDataPathButtonClick(object sender, EventArgs e)
        {
            SetExcelDataDirectory(SelectFolder("Excel数据目录", _excelContext.ExcelDataDirectory));
        }
        
        /// <summary>
        /// 选择二进制输出文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectExportBinaryPathButtonClick(object sender, EventArgs e)
        {
            SetBinaryOutputDirectory(SelectFolder("选择二进制输出文件夹", _excelContext.ClientDataOutDirectory));
        }    
        
        //选择不同的输出类型
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllButtonClick(object sender, EventArgs e)
        {
            for (int i = 0; i < TableCheckBoxList.Items.Count; i++)
            {
                if (!TableCheckBoxList.GetItemChecked(i))
                {
                    TableCheckBoxList.SetItemChecked(i, true);
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButtonClick(object sender, EventArgs e)
        {
            for (int i = 0; i < TableCheckBoxList.Items.Count; i++)
            {
                if (TableCheckBoxList.GetItemChecked(i))
                {
                    TableCheckBoxList.SetItemChecked(i, false);
                }
            }
        }
        
        /// <summary>
        /// 反选。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReverseButtonClick(object sender, EventArgs e)
        {
            for (int i = 0; i < TableCheckBoxList.Items.Count; i++)
            {
                if (TableCheckBoxList.GetItemChecked(i))
                {
                    TableCheckBoxList.SetItemChecked(i, false);
                }
                else
                {
                    TableCheckBoxList.SetItemChecked(i, true);
                }
            }
        }
        
        /// <summary>
        /// 选择json文件的输出目录。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectJsonOutputButtonClick(object sender, EventArgs e)
        {
            SetJsonOutputDirectory(SelectFolder("选择json文件的输出目录", _excelContext.ServerDataOutDirectory));
        }
        
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportButtonClick(object sender, EventArgs e)
        {
            LogOutputText.Text = "";
            ExportButton.Text = "导出中...";
            DisableAllButton();
            Stopwatch stopwatch = Stopwatch.StartNew();
            double startMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;

            _excelContext.Load(GetCurrentSelectFileInfos());
            
            bool exportServerJson = false;
            bool exportClientBinary = false;
            bool exportClientJson = false;
            
            if (JsonCheckBox.Checked && Directory.Exists(JsonOutPath.Text))
            {
                exportServerJson = true;
            }

            if (BinaryCheckBox.Checked && Directory.Exists(BinaryOutPath.Text))
            {
                exportClientBinary = true;
            }

            if (ExportEnumCodeCheckbox.Checked && Directory.Exists(CsEnumCodeOutputPath.Text))
            {
                _excelContext.ExportAllEnumCode();
            }

            if (ExportTableCodeCheckBox.Checked && Directory.Exists(CsTableCodeOutputPath.Text))
            {
                _excelContext.ExportAllDatatable();
            }

            if (ClientJsonCheckBox.Checked)
            {
                exportClientJson = true;
            }
            _excelContext.ExportData(exportServerJson, exportClientBinary, exportClientJson);

            stopwatch.Stop();
            ExportButton.Text = "导出";
            EnableAllButton();
            double endMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
            WriteLog(null, LogLevel.Debug, "导出完成! ");
            WriteLog(null, LogLevel.Debug, $"内存使用: {Math.Max(0, endMemory - startMemory)} M");
            WriteLog(null, LogLevel.Debug, $"耗时: {stopwatch.ElapsedMilliseconds / 1000} 秒");
        }

        /// <summary>
        /// 选择CS枚举文件的输出目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectEnumCodeOutputPath(object sender, EventArgs e)
        {
            SetCsEnumCodeOutputDirectory(SelectFolder("选择CS枚举文件的输出目录", _excelContext.CsharpEnumCodeOutDirectory));
        }
        
        /// <summary>
        /// 选择CS表文件的输出目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTableCodeOutputPath(object sender, EventArgs e)
        {
            SetCsTableCodeOutputDirectory(SelectFolder("选择CS表文件的输出目录", _excelContext.CsharpTableCodeOutDirectory));
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 控制台日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        private void WriteLog(object sender, LogLevel logLevel, string message)
        {
            if (logLevel == LogLevel.Error)
            {
                Color oldColor = LogOutputText.ForeColor;
                LogOutputText.SelectionColor = Color.Red;
                LogOutputText.AppendText($"[{logLevel.ToString()}] ");
                LogOutputText.SelectionColor = oldColor;
                LogOutputText.AppendText($"{message} \r\n");
            }
            else
            {
                LogOutputText.AppendText($"[{logLevel.ToString()}] {message} \r\n");    
            }
            LogOutputText.Select(LogOutputText.TextLength, 0);
            LogOutputText.ScrollToCaret();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
               
        }

        private void ExcelPathText_Click(object sender, EventArgs e)
        {
        }

        private void ControlForm_Load_1(object sender, EventArgs e)
        {
        
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void CsTableCodeOutputPath_TextChanged(object sender, EventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void DisableAllButton()
        {
            ResetAllButton.Enabled = false;
            SelectAllbutton.Enabled = false;
            ReverseSelectButton.Enabled = false;
            ExportButton.Enabled = false;
            ExportTestCodeButton.Enabled = false;
        }

        private void EnableAllButton()
        {
            ResetAllButton.Enabled = true;
            SelectAllbutton.Enabled = true;
            ReverseSelectButton.Enabled = true;
            ExportButton.Enabled = true;
            ExportTestCodeButton.Enabled = true;
        }

        private void ExportTestCodeButton_Click(object sender, EventArgs e)
        {
            DisableAllButton();
            _excelContext.Load(GetCurrentSelectFileInfos());
            _excelContext.ExportTestCode();
            EnableAllButton();
        }

        private void FilterExcelButton_Click(object sender, EventArgs e)
        {
            SetExcelDataDirectory(_iniFile.IniReadValue(IniSectionName, IniNameOfExcelDataDirectory, _excelContext.ExcelDataDirectory));
        }

        private void CsCodeNamespace_TextChanged(object sender, EventArgs e)
        {
            SetCsCodeNamespace(CsNamespaceEdit.Text);
        }

        private void OpenEnumPathButton_Click(object sender, EventArgs e)
        {
            SetEnumDirectory(SelectFolder("枚举目录", _excelContext.EnumDirectory));
        }

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
        }
    }
}
