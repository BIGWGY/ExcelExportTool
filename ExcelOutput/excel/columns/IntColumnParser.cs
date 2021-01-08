using System;
using System.IO;

namespace ExcelTool
{
    public class IntColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Int;
        }

        public string ToCSharpTypeString()
        {
            return "int";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ?  0 : (int)double.Parse(value);
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write((int)double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt32()";
        }
    }
}