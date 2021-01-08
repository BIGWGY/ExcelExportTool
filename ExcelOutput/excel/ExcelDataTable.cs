using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace ExcelTool
{
    public class ExcelDataTable : IComparable<ExcelDataTable>
    {
        /// <summary>
        /// 配置数据的表格名字。
        /// </summary>
        public const string MainSheetName = "Main";

        /// <summary>
        /// 表格数据约束表名。
        /// </summary>
        public const string SettingSheetName = "Setting";

        /// <summary>
        /// 数据起始行(0-based)。
        /// </summary>
        public const int DataColumnStartRow = 4;

        /// <summary>
        /// 文件路径。
        /// </summary>
        private string _filepath;

        /// <summary>
        /// 数据表名。
        /// </summary>
        private string _dataFileName;

        /// <summary>
        /// 多少数据行。
        /// </summary>
        private int _dataRowCount;

        /// <summary>
        /// 字段信息。
        /// </summary>
        private List<ColumnInfo> _columnInfos = new List<ColumnInfo>();

        /// <summary>
        /// 表格约束。
        /// </summary>
        private List<TableConstraint> _tableConstraints = new List<TableConstraint>();

        /// <summary>
        /// 配置错误数量。
        /// </summary>
        private int _error = 0;

        public ExcelDataTable(string filepath, string dataFileName)
        {
            _filepath = filepath;
            _dataFileName = dataFileName;
        }

        public int Error
        {
            get => _error;
            set => _error = value;
        }

        public int DataRowCount
        {
            get => _dataRowCount;
            set => _dataRowCount = value;
        }

        public string Filepath
        {
            get => _filepath;
        }

        public List<ColumnInfo> ColumnInfos
        {
            get => _columnInfos;
        }

        public List<ColumnInfo> GetColumnInfos(ColumnBelong columnBelong)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            foreach (var columnInfo in _columnInfos)
            {
                if (columnInfo.Belong == columnBelong || columnInfo.Belong == ColumnBelong.All)
                {
                    list.Add(columnInfo);
                }
            }
            return list;
        }

        public List<TableConstraint> TableConstraints
        {
            get => _tableConstraints;
        }

        public string DataFileName
        {
            get => _dataFileName;
        }
        
        /// <summary>
        /// 返回主键字段的信息。
        /// </summary>
        /// <returns></returns>
        public ColumnInfo GetPrimaryColumnInfo()
        {
            foreach (var columnAttribute in _columnInfos)
            {
                if (columnAttribute.PrimaryKey)
                {
                    return columnAttribute;
                }
            }

            return null;
        }

        /// <summary>
        /// 返回字段信息。
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private ColumnInfo GetColumnInfo(string columnName)
        {
            foreach (var columnInfo in _columnInfos)
            {
                if (columnInfo.Name.Equals(columnName))
                {
                    return columnInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 返回字段信息。
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private ColumnInfo GetColumnInfo(int columnIndex)
        {
            foreach (var columnInfo in _columnInfos)
            {
                if (columnInfo.ColumnIndex == columnIndex)
                {
                    return columnInfo;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 数据表是否有这个字段名。
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool HasColumnName(string columnName)
        {
            return GetColumnInfo(columnName) != null;
        }

        /// <summary>
        /// 返回字段所在的列。从0开始。
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int GetColumnIndex(string columnName)
        {
            ColumnInfo columnInfo = GetColumnInfo(columnName);
            if (columnInfo == null)
            {
                return -1;
            }

            return columnInfo.ColumnIndex;
        }

        /// <summary>
        /// 表格的某一列是否存在某个值。
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ExistColumnValue(int columnIndex, string value)
        {
            ColumnInfo columnInfo = GetColumnInfo(columnIndex);
            if (columnInfo == null)
            {
                return false;
            }

            return columnInfo.Values.Contains(value);
        }

        /// <summary>
        /// 获取某行某列的数据值。
        /// </summary>
        /// <param name="row">从0开始</param>
        /// <param name="column">从0开始</param>
        /// <returns></returns>
        public string GetColumnValue(int row, int column)
        {
            ColumnInfo columnInfo = GetColumnInfo(column);
            if (columnInfo == null)
            {
                return "";
            }

            return columnInfo.GetRowValue(row);
        }

        public int CompareTo(ExcelDataTable other)
        {
            if (other == null)
            {
                return 1;
            }
            if (other == this)
            {
                return 0;
            }
            if (_dataRowCount < other.DataRowCount)
            {
                return 1;
            }

            return -1;
        }
    }
}