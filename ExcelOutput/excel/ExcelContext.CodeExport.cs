﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using java.awt;
using Microsoft.Extensions.Logging;

namespace ExcelTool
{
    partial class ExcelContext
    {
        /// <summary>
        /// 导出枚举类。
        /// </summary>
        /// <param name="excelEnum"></param>
        public void ExportEnumCode(ExcelEnum excelEnum)
        {
            WriteLog(LogLevel.Debug, $"开始导出枚举表: {excelEnum.EnumClassName}");
            string filename = _csharpEnumCodeOutDirectory 
                              + Path.DirectorySeparatorChar
                              + _codeGenerateClassPrefix
                              + excelEnum.EnumClassName + ".cs";
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                streamWriter.WriteLine($"namespace {_chsarpCodeNameSpace}");
                streamWriter.WriteLine("{");
                if (excelEnum.HeritType.Equals(""))
                {
                    streamWriter.WriteLine($"    public enum {excelEnum.EnumClassName}");    
                }
                else
                {
                    streamWriter.WriteLine($"    public enum {excelEnum.EnumClassName}: {excelEnum.HeritType}");
                }
                streamWriter.WriteLine("    {");
                foreach (var enumInfo in excelEnum.EnumInfos)
                {
                    streamWriter.WriteLine($"        /// <summary>{enumInfo.Description}</summary>");
                    streamWriter.WriteLine($"        {enumInfo.Name} = {enumInfo.Value}, ");
                }
                streamWriter.WriteLine("    }");
                streamWriter.WriteLine("}");
            }
        }

        /// <summary>
        /// 导出所有的枚举表。
        /// </summary>
        public void ExportAllEnumCode()
        {
            foreach (var excelEnum in _enumDictionary.Values)
            {
                ExportEnumCode(excelEnum);
            }
        }
        
        /// <summary>
        /// 导出CS加载byte文件的测试代码。
        /// </summary>
        public void ExportTestCode()
        {
            WriteLog(LogLevel.Debug, "开始导出测试代码");
            string classname = "StaticDataLoaderTest";
            string filename = _csharpTableCodeOutDirectory
                              + Path.DirectorySeparatorChar 
                              + classname + ".cs";
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                streamWriter.WriteLine("using System;");
                streamWriter.WriteLine("using System.IO;");
                streamWriter.WriteLine("");
                streamWriter.WriteLine($"namespace {_chsarpCodeNameSpace}");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine($"    public static class {classname}");
                streamWriter.WriteLine("     {");
                streamWriter.WriteLine("          public static void LoadTest()");
                streamWriter.WriteLine("          {");
                foreach (var table in _excelDataTables.Values)
                {
                    streamWriter.WriteLine($"                StaticDataLoader<{StringUtils.ToCamel(table.DataFileName)}>.GetList(); ");    
                }
                streamWriter.WriteLine("          }");
                streamWriter.WriteLine("     }");
                streamWriter.WriteLine("}");
            }
            WriteLog(LogLevel.Debug, "测试代码导出完成!");
        }
        
        /// <summary>
        /// 导出cs代码。
        /// </summary>
        /// <param name="excelDataTable"></param>
        public void ExportDataTable(ExcelDataTable excelDataTable)
        {
            WriteLog(LogLevel.Debug, $"开始导出数据表: {excelDataTable.DataFileName}");
            string classname = StringUtils.ToCamel(excelDataTable.DataFileName);
            string filename = _csharpTableCodeOutDirectory
                              + Path.DirectorySeparatorChar 
                              + _codeGenerateClassPrefix
                              + classname + ".cs";
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                streamWriter.WriteLine("using System;");
                streamWriter.WriteLine("using System.IO;");
                streamWriter.WriteLine("using System.Linq;");
                streamWriter.WriteLine("using System.Collections.Generic;");
                streamWriter.WriteLine("");
                streamWriter.WriteLine($"namespace {_chsarpCodeNameSpace}");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine($"    public class {classname}: DataRow");
                streamWriter.WriteLine("    {");
                // 字段.
                foreach (var columnInfo in excelDataTable.GetColumnInfos(ColumnBelong.Client))
                {
                    IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                    streamWriter.WriteLine($"        /// <summary> {columnInfo.Title} </summary>");
                    streamWriter.WriteLine($"        public {parser.ToCSharpTypeString()} {columnInfo.Name}");
                    streamWriter.WriteLine( "        {");
                    streamWriter.WriteLine( "            get;");
                    streamWriter.WriteLine( "            private set;");
                    streamWriter.WriteLine( "        }");
                }
                // 解析函数
                streamWriter.WriteLine($"        public override void ReadRowData(BinaryReader reader)");
                streamWriter.WriteLine( "        {");
                foreach (var columnInfo in excelDataTable.GetColumnInfos(ColumnBelong.Client))
                {
                    IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                    streamWriter.WriteLine($"            {columnInfo.Name} = {parser.ReadFromBinaryReaderExpression()} ;" );
                }
                streamWriter.WriteLine( "        }");
                // 主键函数
                ColumnInfo pkInfo = excelDataTable.GetPrimaryColumnInfo();
                streamWriter.WriteLine( "        public override int GetPk()");
                streamWriter.WriteLine( "        {");
                if (pkInfo == null)
                {
                    streamWriter.WriteLine($"            return 0;");    
                }
                else
                {
                    streamWriter.WriteLine($"            return {pkInfo.Name};");
                }
                streamWriter.WriteLine( "        }");
                streamWriter.WriteLine("    }");
                streamWriter.WriteLine("}");
            }
        }
        
        /// <summary>
        /// 导出所有的枚举表。
        /// </summary>
        public void ExportAllDatatable()
        {
            List<Task> tasks = new List<Task>();
            foreach (var datatable in _excelDataTables.Values)
            {
                tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        ExportDataTable(datatable);
                    }, CancellationToken.None, TaskCreationOptions.None, _scheduler)
                );
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}