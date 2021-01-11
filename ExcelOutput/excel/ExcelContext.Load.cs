using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExcelTool
{
    public partial class ExcelContext
    {
        private void AddExcelTable(ExcelDataTable excelDataTable)
        {
            Monitor.Enter(LoadTableBytes);
            _excelDataTables.Add(excelDataTable.DataFileName, excelDataTable);
            Monitor.Exit(LoadTableBytes);
        }

        private void BeforeLoad()
        {
            CheckConfigDirectory();

            EnsureOutputDirectoryExist();

            _selectExcelFileInfos.Clear();

            _enumDictionary.Clear();

            _excelDataTables.Clear();
        }

        public void LoadTest()
        {
            BeforeLoad();

            _selectExcelFileInfos = FilterFile(_excelExcelDataDirectory, ExcelContext.ExcelSuffix);
            
            LoadAllEnumTables();

            LoadDataTable();
            
            CheckAllTable();

            foreach (var table in _excelDataTables.Values)
            {
                if (table.DataFileName.Contains("static_vip_template")) 
                {
                    ExcelDataTableCheck check = new ExcelDataTableCheck(this, table);
                    check.CheckDataTable();    
                }
            }
        }

        public void Load(List<FileInfo> excelDataFileInfos)
        {
            BeforeLoad();

            _selectExcelFileInfos = excelDataFileInfos;

            LoadAllEnumTables();

            LoadDataTable();

            CheckAllTable();
        }

        public static List<FileInfo> FilterFile(string directory, string extension)
        {
            if (!Directory.Exists(directory))
            {
                return new List<FileInfo>();
            }
            DirectoryInfo di = new DirectoryInfo(directory);
            return di.GetFiles().Where(f => f.Name.EndsWith(extension)).ToList();
        }

        /// <summary>
        /// 加载所有的表格数据。
        /// </summary>
        private void LoadDataTable()
        {
            logMsgHandler(null, LogLevel.Debug, $"当期加载{_selectExcelFileInfos.Count}张数据表格!");

            _selectExcelFileInfos.Sort((x, y) =>
            {
                if (x.Length < y.Length)
                {
                    return 1;
                }

                return -1;
            });

            List<Task> tasks = new List<Task>();

            foreach (var fileInfo in _selectExcelFileInfos)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                    {
                        WriteLog(LogLevel.Information, $"加载文件: {fileInfo.Name}, 大小 {fileInfo.Length / 1000} k ");
                        try
                        {
                            ExcelTableLoader loader = new ExcelTableLoader(this, fileInfo.FullName, fileInfo.Name);
                            loader.Load();
                            AddExcelTable(loader.ExcelDataTable);
                        }
                        catch (Exception e)
                        {
                            WriteLog(LogLevel.Error, $"表格 {fileInfo.Name} 加载失败: {e.Message}");
                        }
                    }, CancellationToken.None, TaskCreationOptions.PreferFairness, _scheduler)
                );
            }

            Task.WaitAll(tasks.ToArray());

            WriteLog(LogLevel.Debug, $"总共加载 {_excelDataTables.Count} 个表格!");
        }

        /// <summary>
        /// 加载所有枚举表格。
        /// </summary>
        private void LoadAllEnumTables()
        {
            DirectoryInfo di = new DirectoryInfo(_enumDirectory);

            FileInfo[] fileInfos = di.GetFiles().Where(f => f.Name.EndsWith(ExcelSuffix)).ToArray();

            logMsgHandler(null, LogLevel.Debug, $"当期加载{fileInfos.Length}张枚举表格!");

            foreach (var fileInfo in fileInfos)
            {
                try
                {
                    ExcelEnumTableLoader loader = new ExcelEnumTableLoader(this, fileInfo.FullName, fileInfo.Name);
                    loader.Load();
                    foreach (var pairValue in loader.ExcelEnums)
                    {
                        _enumDictionary.Add(pairValue.Key, pairValue.Value);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    logMsgHandler(null, LogLevel.Debug, $"表格{fileInfo.Name}加载失败!");
                }
            }
        }
    }
}