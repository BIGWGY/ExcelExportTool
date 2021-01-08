using System;
using System.IO;

namespace ExcelTool
{
    public class Int16ColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Int16;
        }

        public string ToCSharpTypeString()
        {
            return "Int16";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (short) 0 : Convert.ToInt16(double.Parse(value));
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(Convert.ToInt16(double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value)));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt16()";
        }
    }
}