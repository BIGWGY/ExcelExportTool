using System.Collections.Generic;

namespace ExcelTool
{
    public class ColumnInfo
    {
        /// <summary>
        /// 是否为主键。
        /// </summary>
        private bool _primaryKey;
        
        /// <summary>
        /// 属性。
        /// </summary>
        private ColumnBelong _belong;

        /// <summary>
        /// 字段类型。
        /// </summary>
        private ColumnType _columnType;

        /// <summary>
        /// 字段说明。
        /// </summary>
        private string _title;

        /// <summary>
        /// 英文名称。
        /// </summary>
        private string _name;

        /// <summary>
        /// 字段在表格中第几列（从0开始）。
        /// </summary>
        private int _columnIndex;

        /// <summary>
        /// 这一列的所有有效行的数据.
        /// </summary>
        private List<string> _values = new List<string>();

        public List<string> Values
        {
            get => _values;
            set => _values = value;
        }

        public ColumnBelong Belong
        {
            get => _belong;
            set => _belong = value;
        }

        public ColumnType ColumnType
        {
            get => _columnType;
            set => _columnType = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int ColumnIndex
        {
            get => _columnIndex;
            set => _columnIndex = value;
        }

        public bool PrimaryKey
        {
            get => _primaryKey;
            set => _primaryKey = value;
        }

        /// <summary>
        /// 获取某行的值。
        /// </summary>
        /// <param name="row">从0开始</param>
        /// <returns></returns>
        public string GetRowValue(int row)
        {
            if (row < 0 || row >= _values.Count)
            {
                return "";
            }

            return _values[row];
        }

        /// <summary>
        /// 是否为客户端字段。
        /// </summary>
        /// <returns></returns>
        public bool IsClient()
        {
            if (_belong == ColumnBelong.All || _belong == ColumnBelong.Client)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// 是否为服务器字段
        /// </summary>
        /// <returns></returns>
        public bool IsServer()
        {
            if (_belong == ColumnBelong.All || _belong == ColumnBelong.Server)
            {
                return true;
            }

            return false;
        }
    }
}