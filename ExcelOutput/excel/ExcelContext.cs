using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ExcelTool
{
    /// <summary>
    /// 分析数据产生的日志信息。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    public delegate void LogMsgHandler(object sender, LogLevel logLevel, string message);

    public partial class ExcelContext
    {
        /// <summary>
        /// 枚举表格所在目录。
        /// </summary>
        private string _enumDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "enum";
        
        /// <summary>
        /// 数据表格所在目录。
        /// </summary>
        private string _excelExcelDataDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "excel";

        /// <summary>
        /// 导出的客户端数据存储目录。
        /// </summary>
        private string _clientDataOutDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "client";

        /// <summary>
        /// 导出的服务端数据存储目录。
        /// </summary>
        private string _serverDataOutDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "server";

        /// <summary>
        /// 导出的客户端代码存储目录。
        /// </summary>
        private string _csharpTableCodeOutDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "csharptable";

        /// <summary>
        /// 导出的枚举代码放置的目录。 
        /// </summary>
        private string _csharpEnumCodeOutDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "csharpenumcode";

        /// <summary>
        /// 导出 csharp 代码的命名空间。 
        /// </summary>
        private string _chsarpCodeNameSpace = "DyspaceWork";
        
        /// <summary>
        /// 生成类的前缀。
        /// </summary>
        private const string _codeGenerateClassPrefix = "";
        
        /// <summary>
        /// 线程调度。
        /// </summary>
        private static LimitedTaskScheduler _scheduler = new LimitedTaskScheduler();
        
        /// <summary>
        /// 字段解析辅助类。
        /// </summary>
        private ColumnParserHelp _columnParserHelp = new ColumnParserHelp();

        /// <summary>
        /// 已选择的导出的excel文件。 
        /// </summary>
        private List<FileInfo> _selectExcelFileInfos = new List<FileInfo>();
        
        /// <summary>
        /// 已定义的所有枚举类型。
        /// </summary>
        private Dictionary<string, ExcelEnum> _enumDictionary = new Dictionary<string, ExcelEnum>();
        
        /// <summary>
        /// 已定义的所有表格。
        /// </summary>
        private Dictionary<string, ExcelDataTable> _excelDataTables = new Dictionary<string, ExcelDataTable>();

        /// <summary>
        /// excel 文件后缀。
        /// </summary>
        public const string ExcelSuffix = ".xlsx";
        
        private byte[] LoadTableBytes = new byte[1];
        
        private byte[] LoadLogBytes = new byte[1];

        /// <summary>
        /// 运行日志输出。
        /// </summary>
        public LogMsgHandler logMsgHandler = (sender, level, message) => { };

        public Dictionary<string, ExcelDataTable> ExcelDataTables
        {
            get => _excelDataTables;
            set => _excelDataTables = value;
        }

        public Dictionary<string, ExcelEnum> EnumDictionary
        {
            get => _enumDictionary;
            set => _enumDictionary = value;
        }

        public string EnumDirectory
        {
            get => _enumDirectory;
            set => _enumDirectory = value;
        }

        public string ExcelDataDirectory
        {
            get => _excelExcelDataDirectory;
            set => _excelExcelDataDirectory = value;
        }

        public string ClientDataOutDirectory
        {
            get => _clientDataOutDirectory;
            set => _clientDataOutDirectory = value;
        }

        public string ServerDataOutDirectory
        {
            get => _serverDataOutDirectory;
            set => _serverDataOutDirectory = value;
        }

        public string CsharpTableCodeOutDirectory
        {
            get => _csharpTableCodeOutDirectory;
            set => _csharpTableCodeOutDirectory = value;
        }

        public string CsharpEnumCodeOutDirectory
        {
            get => _csharpEnumCodeOutDirectory;
            set => _csharpEnumCodeOutDirectory = value;
        }

        public string ChsarpCodeNameSpace
        {
            get => _chsarpCodeNameSpace;
            set => _chsarpCodeNameSpace = value;
        }

        /// <summary>
        /// 检查数据目录。
        /// </summary>
        public void CheckConfigDirectory()
        {
            if (!Directory.Exists(_excelExcelDataDirectory))
            {
                MessageBox.Show($"数据目录 {_excelExcelDataDirectory} 不存在!");
                Directory.CreateDirectory(_excelExcelDataDirectory);
            }

            if (!Directory.Exists(_enumDirectory))
            {
                Directory.CreateDirectory(_enumDirectory);
            }
        }
        
        /// <summary>
        /// 检查配置目录是否存在，如果不存在，则创建。
        /// </summary>
        public void EnsureOutputDirectoryExist()
        {
            if (!Directory.Exists(_clientDataOutDirectory))
                Directory.CreateDirectory(_clientDataOutDirectory);
            if (!Directory.Exists(_serverDataOutDirectory)) 
                Directory.CreateDirectory(_serverDataOutDirectory);
            if (!Directory.Exists(_csharpTableCodeOutDirectory))
                Directory.CreateDirectory(_csharpTableCodeOutDirectory);
            if (!Directory.Exists(_csharpEnumCodeOutDirectory))
                Directory.CreateDirectory(_csharpEnumCodeOutDirectory);
        }

        /// <summary>
        /// 调试信息。
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void WriteLog(LogLevel logLevel, string message)
        {
            Monitor.Enter(LoadLogBytes);
            logMsgHandler(null, logLevel, message);
            Monitor.Exit(LoadLogBytes);
        }
    }
}