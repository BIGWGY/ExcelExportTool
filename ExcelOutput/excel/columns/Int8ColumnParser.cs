using System;
using System.IO;

namespace ExcelTool
{
    public class Int8ColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Int8;
        }

        public string ToCSharpTypeString()
        {
            return "byte";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (short)0 : Convert.ToInt16(double.Parse(value));
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(byte.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadByte()";
        }
    }
}