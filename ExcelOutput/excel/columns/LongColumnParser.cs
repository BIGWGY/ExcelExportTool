using System;
using System.IO;

namespace ExcelTool
{
    public class LongColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Long;
        }

        public string ToCSharpTypeString()
        {
            return "long";
        }

        public string ToJavaTypeString()
        {
            return "long";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (long) 0 : (long)double.Parse(value);
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write((long)double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt64()";
        }
    }
}