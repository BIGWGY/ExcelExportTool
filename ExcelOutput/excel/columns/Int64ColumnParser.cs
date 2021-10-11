using System;
using System.IO;

namespace ExcelTool
{
    public class Int64ColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Int64;
        }

        public string ToCSharpTypeString()
        {
            return "Int64";
        }

        public string ToJavaTypeString()
        {
            return "long";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (Int64) 0 : Convert.ToInt64(double.Parse(value));
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(Convert.ToInt64(double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value)));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt64()";
        }
    }
}