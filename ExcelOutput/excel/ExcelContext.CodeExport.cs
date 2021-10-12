using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ikvm.extensions;
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
                streamWriter.WriteLine("using UnityGameFramework.Runtime;");
                
                streamWriter.WriteLine("");
                streamWriter.WriteLine($"namespace {_chsarpCodeNameSpace}");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine($"\tpublic partial class {classname}: DataRowBase");
                streamWriter.WriteLine("\t{");
                // 字段.
                foreach (var columnInfo in excelDataTable.GetColumnInfos(ColumnBelong.Client))
                {
                    IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                    streamWriter.WriteLine($"\t\t/// <summary> {columnInfo.Title} </summary>");
                    if ("Id".Equals(columnInfo.Name))
                    {
                        streamWriter.WriteLine("\t\tprivate int m_Id = 0;");
                        streamWriter.WriteLine("\t\tpublic override int Id");
                        streamWriter.WriteLine("\t\t{");
                        streamWriter.WriteLine("\t\t\tget { return m_Id; }");
                        // streamWriter.WriteLine("\t\t}");
                    }
                    else
                    {
                        streamWriter.WriteLine($"\t\tpublic {parser.ToCSharpTypeString()} {StringUtils.ToCamel(columnInfo.Name)}");
                        streamWriter.WriteLine( "\t\t{");
                        streamWriter.WriteLine( "\t\t\tget;");
                    }
                    if (!"Id".Equals(columnInfo.Name))
                    {
                        streamWriter.WriteLine( "\t\t\tprivate set;");    
                    }
                    streamWriter.WriteLine( "\t\t}");
                    streamWriter.WriteLine( "");
                }
                
                // 行字符串解析函数
                streamWriter.WriteLine("\t\tpublic virtual bool ParseDataRow(string dataRowString, object userData)");
                streamWriter.WriteLine( "\t\t{");
                streamWriter.WriteLine( "\t\t\t throw new Exception(\"not allow call this method!\");");
                streamWriter.WriteLine( "\t\t}");
                streamWriter.WriteLine( "");

                // 行字节流解析函数
                streamWriter.WriteLine("\t\tpublic override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)");
                streamWriter.WriteLine( "\t\t{");
                streamWriter.WriteLine("\t\t\tusing (MemoryStream memoryStream = new MemoryStream(dataRowBytes))");
                streamWriter.WriteLine("\t\t\tusing (BinaryReader reader = new BinaryReader(memoryStream))");
                streamWriter.WriteLine("\t\t\t{");
                streamWriter.WriteLine("\t\t\t\t try");
                streamWriter.WriteLine("\t\t\t\t {");
                foreach (var columnInfo in excelDataTable.GetColumnInfos(ColumnBelong.Client))
                {
                    IColumnParser parser = _columnParserHelp.GetColumnParser(columnInfo.ColumnType);
                    if ("Id".Equals(columnInfo.Name))
                    {
                        streamWriter.WriteLine($"\t\t\t\t\tm_Id = {parser.ReadFromBinaryReaderExpression()} ;" );
                    }
                    else
                    {
                        streamWriter.WriteLine($"\t\t\t\t\t {StringUtils.ToCamel(columnInfo.Name)} = {parser.ReadFromBinaryReaderExpression()} ;" );    
                    }
                }streamWriter.WriteLine("\t\t\t\t\t return true;");
                streamWriter.WriteLine("\t\t\t\t }");
                streamWriter.WriteLine("\t\t\t\t catch (Exception e)");
                streamWriter.WriteLine("\t\t\t\t {");
                streamWriter.WriteLine("\t\t\t\t\t Console.WriteLine(e);");
                streamWriter.WriteLine("\t\t\t\t\t return false;");
                streamWriter.WriteLine("\t\t\t\t }");
                streamWriter.WriteLine( "\t\t\t}");
                streamWriter.WriteLine("\t\t}");
                streamWriter.WriteLine("\t}");
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
            
            CopyTemplateCode();
        }

        /// <summary>
        /// 拷贝模版
        /// </summary>
        private void CopyTemplateCode()
        {
            List<FileInfo> fileInfos = FilterFile(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "template", ".cs");
            foreach (var csFile in fileInfos)
            {
                ReplaceAndCopyFile(csFile.FullName, _csharpTableCodeOutDirectory, "__NAMESPACE__", _chsarpCodeNameSpace);
            }
        }
        
        /// <summary>
        /// 替换文件指定字符串。
        /// </summary>
        /// <param name="srcFilePath"></param>
        /// <param name="dstDirectory"></param>测试代码导出完成
        /// <param name="search"></param>
        /// <param name="replace"></param>
        public static void ReplaceAndCopyFile(string srcFilePath, string dstDirectory, string search, string replace)
        {
            if (!File.Exists(srcFilePath) || !Directory.Exists(dstDirectory))
            {
                return;
            }

            string filename = Path.GetFileName(srcFilePath);
            
            using (StreamReader reader = new StreamReader(srcFilePath, Encoding.UTF8))
            using (StreamWriter writer = new StreamWriter(dstDirectory + Path.DirectorySeparatorChar + filename, false, Encoding.UTF8))
            {
                string content = reader.ReadToEnd().replaceAll(search, replace);
                writer.Write(content);
            }    
        }
    }
}