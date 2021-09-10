using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExcelTool
{
    partial class ExcelContext
    {
        /// <summary>
        /// 导出数据 
        /// </summary>
        /// <param name="exportServerJson"></param>
        /// <param name="exportClientBinary"></param>
        /// <param name="exportClientJson"></param>
        public void ExportData(bool exportServerJson, bool exportClientBinary, bool exportClientJson)
        {
            if (exportServerJson == false && exportClientBinary == false && exportClientJson == false)
            {
                return;
            }

            _tempClientJsonDictionary.Clear();
            
            List<ExcelDataTable> list = new List<ExcelDataTable>(_excelDataTables.Values);
            list.Sort();
            
            List<Task> tasks = new List<Task>();
            foreach (var excelDataTable in list)
            {
                if (excelDataTable.Error > 0)
                {
                    WriteLog(LogLevel.Error, $"表格 {excelDataTable.DataFileName} 有错误 {excelDataTable.Error} 个， 未导出表格!");
                    continue;
                }
                
                tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        if (exportServerJson)
                        {
                            ExportJson(excelDataTable.DataFileName, ColumnBelong.Server);
                        }

                        if (exportClientBinary)
                        {
                            ExportBytes(excelDataTable.DataFileName, ColumnBelong.Client);
                        }

                        if (exportClientJson)
                        {
                            ExportClientJson(excelDataTable.DataFileName, ColumnBelong.Client);
                        }
                    }, CancellationToken.None, TaskCreationOptions.PreferFairness, _scheduler)
                );
            }

            Task.WaitAll(tasks.ToArray());

            ExportTempClientJson();
        }

        /// <summary>
        /// 导出客户端的json文件
        /// </summary>
        public void ExportTempClientJson()
        {
            if (_tempClientJsonDictionary.IsEmpty)
            {
                return;
            }
            using (FileStream writeFile = new FileStream(GetClientJsonExportFileName(), FileMode.Create))
            using (TextWriter textWriter = new StreamWriter(writeFile, Encoding.UTF8))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                textWriter.Write(JsonConvert.SerializeObject(_tempClientJsonDictionary, Formatting.None));
                stopwatch.Stop();
                WriteLog(LogLevel.Information, $"线程 {Thread.CurrentThread.ManagedThreadId.ToString()}, 耗时 {stopwatch.ElapsedMilliseconds / 1000} 秒 , 导出 client json 文件: {GetClientJsonExportFileName()}");
            }
            _tempClientJsonDictionary.Clear();
        }

        /// <summary>
        /// 导出二进制字节流。
        /// </summary>
        /// <param name="excelDataTable"></param>
        /// <returns></returns>
        public string GetBinaryExportFileName(ExcelDataTable excelDataTable)
        {
            return _clientDataOutDirectory + Path.DirectorySeparatorChar + excelDataTable.DataFileName + ".byte";
        }

        /// <summary>
        /// 导出json的文件名。
        /// </summary>
        /// <param name="excelDataTable"></param>
        /// <returns></returns>
        public string GetJsonExportFileName(ExcelDataTable excelDataTable)
        {
            return _serverDataOutDirectory + Path.DirectorySeparatorChar + excelDataTable.DataFileName + ".json";
        }
        
        /// <summary>
        /// 客户端导出json文件名字
        /// </summary>
        /// <returns></returns>
        public string GetClientJsonExportFileName()
        {
            return _clientJsonDataOutDirectory + Path.DirectorySeparatorChar + "client.json";
        }
        
        /// <summary>
        /// 导出客户端表的json字符串
        /// </summary>
        /// <param name="dataFileName"></param>
        /// <param name="belong"></param>
        private void ExportClientJson(string dataFileName, ColumnBelong belong)
        {
            if (!_excelDataTables.ContainsKey(dataFileName))
            {
                WriteLog(LogLevel.Error, $"找不到配置表: {dataFileName}");
                return;
            }
            ExcelDataTable dataTable = _excelDataTables[dataFileName];

            List<ColumnInfo> columnInfos = dataTable.GetColumnInfos(belong);
            
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, Object>> list = new List<Dictionary<string, object>>();

            for (int i = 0; i < dataTable.DataRowCount && columnInfos.Count > 0; i++)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (var columnInfo in columnInfos)
                {
                    try
                    {
                        IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                        dictionary.Add(columnInfo.Name, parser.ToObject(columnInfo.GetRowValue(i)));
                    }
                    catch (Exception e)
                    {
                        WriteLog(LogLevel.Error, $"表格 {dataTable.DataFileName} {columnInfo.Name} 字段, {i + 5} 行, 值 {columnInfo.GetRowValue(i)}, 导出失败: {e.Message}");
                        return;
                    }
                }

                list.Add(dictionary);
            }

            _tempClientJsonDictionary.AddOrUpdate(dataFileName, list, (s, s1) => list);
            stopwatch.Stop();
            WriteLog(LogLevel.Information, $"线程 {Thread.CurrentThread.ManagedThreadId.ToString()}, 耗时 {stopwatch.ElapsedMilliseconds / 1000} 秒 , 导出 client json 文件: {dataFileName}");
        }
         
        /// <summary>
        /// 导出 json 文件.
        /// </summary>
        /// <param name="dataFileName"></param>
        public void ExportJson(string dataFileName, ColumnBelong belong)
        {
            if (!_excelDataTables.ContainsKey(dataFileName))
            {
                WriteLog(LogLevel.Error, $"找不到配置表: {dataFileName}");
                return;
            }

            ExcelDataTable dataTable = _excelDataTables[dataFileName];

            List<ColumnInfo> columnInfos = dataTable.GetColumnInfos(belong);

            using (FileStream writeFile = new FileStream(GetJsonExportFileName(dataTable), FileMode.Create))
            using (TextWriter textWriter = new StreamWriter(writeFile, Encoding.UTF8))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                List<Dictionary<string, Object>> list = new List<Dictionary<string, object>>();
                
                for (int i = 0; i < dataTable.DataRowCount && columnInfos.Count > 0; i++)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    foreach (var columnInfo in columnInfos)
                    {
                        try
                        {
                            IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                            dictionary.Add(columnInfo.Name, parser.ToObject(columnInfo.GetRowValue(i)));
                        }
                        catch (Exception e)
                        {
                            WriteLog(LogLevel.Error,
                                $"表格 {dataTable.DataFileName} {columnInfo.Name} 字段, {i + 5} 行, 值 {columnInfo.GetRowValue(i)}, 导出失败: {e.Message}");
                            return;
                        }
                    }

                    list.Add(dictionary);
                }

                textWriter.Write(JsonConvert.SerializeObject(list, Formatting.Indented));
                stopwatch.Stop();
                WriteLog(LogLevel.Information, $"线程 {Thread.CurrentThread.ManagedThreadId.ToString()}, 耗时 {stopwatch.ElapsedMilliseconds / 1000} 秒 , 导出 json 文件: {dataFileName}");
            }
        }

        /// <summary>
        /// 之前旧版的字节流格式。
        /// </summary>
        /// <param name="dataFileName"></param>
        /// <param name="columnBelong"></param>
        public void ExportBytes(string dataFileName, ColumnBelong columnBelong)
        {
            if (!_excelDataTables.ContainsKey(dataFileName))
            {
                WriteLog(LogLevel.Error, $"找不到配置表: {dataFileName}");
                return;
            }

            ExcelDataTable dataTable = _excelDataTables[dataFileName];
            List<ColumnInfo> columnInfos = dataTable.GetColumnInfos(columnBelong);

            try
            {
                using (FileStream writeFile = new FileStream(GetBinaryExportFileName(dataTable), FileMode.Create))
                using (BinaryWriter binaryWriter = new BinaryWriter(writeFile, Encoding.UTF8))
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    for (int i = 0; i < dataTable.DataRowCount; i++)
                    {
                        foreach (var columnInfo in columnInfos)
                        {
                            try
                            {
                                IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                                parser.WriteToBinaryWriter(binaryWriter, columnInfo.GetRowValue(i));
                            }
                            catch (Exception e)
                            {
                                WriteLog(LogLevel.Error,
                                    $"表格 {dataTable.DataFileName} {columnInfo.Name} 字段, {i + 5} 行, 值 {columnInfo.GetRowValue(i)}, 导出失败: {e.Message}");
                                throw e;
                            }
                        }
                    }

                    stopwatch.Stop();
                    WriteLog(LogLevel.Information,
                        $"线程 {Thread.CurrentThread.ManagedThreadId.ToString()}, 耗时 {stopwatch.ElapsedMilliseconds / 1000} 秒 , 导出 byte 文件: {dataFileName}");
                }
            }
            catch (Exception e)
            {
                File.Delete(GetBinaryExportFileName(dataTable));
            }
        }
    }
}