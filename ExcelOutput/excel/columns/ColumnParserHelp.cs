using System.Collections.Generic;

namespace ExcelTool
{
    public class ColumnParserHelp
    {
        private Dictionary<ColumnType, IColumnParser> _parsers = new Dictionary<ColumnType, IColumnParser>();
        
        public ColumnParserHelp()
        {
            _parsers.Add(ColumnType.Byte, new ByteColumnParser()); 
            _parsers.Add(ColumnType.Int8, new Int8ColumnParser());
            _parsers.Add(ColumnType.Int16, new Int16ColumnParser());
            _parsers.Add(ColumnType.Short, new ShortColumnParser());
            _parsers.Add(ColumnType.Int, new IntColumnParser());
            _parsers.Add(ColumnType.Int32, new Int32ColumnParser());
            _parsers.Add(ColumnType.Int64, new Int64ColumnParser());
            _parsers.Add(ColumnType.Long, new LongColumnParser());
            _parsers.Add(ColumnType.Float, new FloatColumnParser());
            _parsers.Add(ColumnType.Double, new DoubleColumnParser());
            _parsers.Add(ColumnType.Time, new TimeColumnParser());
            _parsers.Add(ColumnType.String, new StringColumnParser());
            _parsers.Add(ColumnType.Lang, new LangColumnParser());
            _parsers.Add(ColumnType.IntArray, new IntArrayColumnParser());
            _parsers.Add(ColumnType.IntIntMap, new IntIntMap());
            _parsers.Add(ColumnType.StringArray, new StringArrayColumnParser());
        }

        public IColumnParser GetColumnParser(ColumnType columnType)
        {
            return _parsers[columnType];
        }
    }
}