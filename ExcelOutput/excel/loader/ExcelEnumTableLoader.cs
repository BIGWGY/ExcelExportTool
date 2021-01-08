using System.Collections.Generic;
using System.IO;
using ikvm.extensions;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;

namespace ExcelTool
{
    public class ExcelEnumTableLoader: ExcelLoader
    {
        /// <summary>
        /// 加载的文件路径。
        /// </summary>
        private string _filepath;
        
        /// <summary>
        /// 文件名。
        /// </summary>
        private string _filename;

        private IWorkbook _workbook;
        
        /// <summary>
        /// 文件里面包含的枚举类。
        /// </summary>
        private Dictionary<string, ExcelEnum> _excelEnums = new Dictionary<string, ExcelEnum>();

        public Dictionary<string, ExcelEnum> ExcelEnums
        {
            get => _excelEnums;
        }

        public ExcelEnumTableLoader(ExcelContext excelContext, string path, string filename)
        {
            _filepath = path;
            _filename = filename;
            _excelContext = excelContext;
        }

        /// <summary>
        /// 读取枚举值。
        /// </summary>
        /// <param name="enumPair"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private EnumInfo ParseEnumAttribute(Dictionary<string, int> enumPair, IRow row)
        {
            if (row == null || row.Cells.Count < 3)
            {
                return null;
            }
                
            string enumName = row.GetCell(0).StringCellValue.Trim();
            if (enumPair.ContainsKey(enumName)) 
            {
                AddDebugInfo(LogLevel.Error, $"file {_filename} repeat enum {enumName}");
                return null;
            }

            int enumValue = (int)row.GetCell(1).NumericCellValue;
            if (enumPair.ContainsValue(enumValue))
            {
                AddDebugInfo(LogLevel.Error, $"file {_filename} repeat enum value {enumName}, {enumValue}");
                return null;
            }
            
            EnumInfo info = new EnumInfo();
            info.Name = enumName;
            info.Value = enumValue;
            info.Description = GetCellString(row.GetCell(2));
            return info;
        }
        
        /// <summary>
        /// 读取枚举类。
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private ExcelEnum ParseExcelEnum(ISheet sheet)
        {
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null || headerRow.GetCell(0) == null || headerRow.GetCell(0).StringCellValue.isEmpty())
            {
                AddDebugInfo(LogLevel.Error, $"枚举表 {_filename} 名字配置错误!");
                return null;
            }
            ICell headerFirstCell = headerRow.GetCell(0);
            
            string[] tableInfos = headerFirstCell.StringCellValue.split("#");
            if (tableInfos.Length <= 0 || tableInfos[0].Equals(""))
            {
                AddDebugInfo(LogLevel.Error, $"枚举表 {_filename} 名字配置错误!");
                return null;
            }
            string enumClassName = tableInfos[0].substring(0, 1).toUpperCase() + tableInfos[0].substring(1);

            ExcelEnum excelEnum = new ExcelEnum();
            
            excelEnum.Filepath = _filepath;
            excelEnum.EnumClassName = enumClassName;
            
            Dictionary<string, int> enumPair = new Dictionary<string, int>();
            for (int i = 1; i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                EnumInfo info = ParseEnumAttribute(enumPair, row);
                if (info != null)
                {
                    if (excelEnum.HasEnumValue(info.Value))
                    {
                        AddDebugInfo(LogLevel.Error, $"枚举表 {_filename} 重复值: {info.Value}");
                        continue;
                    }
                    if (excelEnum.HasEnumName(info.Name))
                    {
                        AddDebugInfo(LogLevel.Error, $"枚举表 {_filename} 重复枚举名字: {info.Name}");
                        continue;
                    }
                    excelEnum.EnumInfos.Add(info);                
                }
            }

            if (tableInfos.Length == 2)
            {
                if (tableInfos[1].Equals("byte") || tableInfos[1].Equals("int"))
                {
                    excelEnum.HeritType = tableInfos[1];
                }
            }
            
            return excelEnum;
        }
 
        /// <summary>
        /// 读取枚举文件。
        /// </summary>
        public void Load()
        {
            using (FileStream stream = new FileStream(_filepath, FileMode.Open, FileAccess.Read))
            {
                _workbook = WorkbookFactory.Create(stream);    
                
                for (int i = 0; i < _workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = _workbook.GetSheetAt(i);
                    if (sheet == null)
                    {
                        continue;
                    }
                    string[] nameList = sheet.SheetName.Split('#');
                    if (nameList.Length != 2)
                    {
                        continue;
                    }
                    if (!"enum".Equals(nameList[0]) && !"枚举".Equals(nameList[0]))
                    {
                        continue;
                    }
                    ExcelEnum excelEnum = ParseExcelEnum(sheet);
                    if (excelEnum != null)
                    {
                        _excelEnums.Add(excelEnum.EnumClassName, excelEnum);
                    }
                }
                _workbook.Close();
            }
        }
    }
}