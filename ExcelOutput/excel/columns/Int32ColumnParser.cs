using System;
using System.IO;

namespace ExcelTool
{
    public class Int32ColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Int32;
        }

        public string ToCSharpTypeString()
        {
            return "Int32";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ?  0 : Convert.ToInt32(double.Parse(value));
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(Convert.ToInt32(double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value)));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt32()";
        }
    }
}