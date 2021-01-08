using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.Util;

namespace ExcelTool
{
    public abstract class ExcelLoader
    {
        protected ExcelContext _excelContext;
        
        /// <summary>
        /// 添加调试信息。
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        protected void AddDebugInfo(LogLevel logLevel, string message)
        {
            _excelContext.WriteLog(logLevel, message);
        }

        /// <summary>
        /// 返回单元格的字符串。
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public string GetCellString(ICell cell)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                return "";
            }
            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue;
            }
            cell.SetCellType(CellType.String);
            string value = cell.StringCellValue;
            return value;
        }

        /// <summary>
        /// 获取字段归属类型。
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeException"></exception>
        public ColumnBelong GetColumnBelong(string literal)
        {
            foreach (ColumnBelong belong in typeof(ColumnBelong).GetEnumValues())
            {
                if (belong.ToString().ToLower().Equals(literal.ToLower()))
                {
                    return belong;
                }
            }
            
            throw new RuntimeException($"not found enum type {literal}");
        }

        /// <summary>
        /// 返回字段数据类型。
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeException"></exception>
        public ColumnType GetColumnType(string literal)
        {
            foreach (ColumnType columnType in typeof(ColumnType).GetEnumValues())
            {
                if (columnType.ToString().ToLower().Equals(literal.ToLower()))
                {
                    return columnType;
                }
            }
            
            throw new RuntimeException($"没有找到对应的字段数据类型 {literal}");
        }
 
        /// <summary>
        /// 是否为空行。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool IsEmptyRow(IRow row)
        {
            if (row == null)
            {
                return true;
            }
            for (int i = 0; i < row.LastCellNum; i++)
            {
                if (!GetCellString(row.GetCell(i)).Equals(""))
                {
                    return false;
                }
            }

            return true;
        }
    }
}