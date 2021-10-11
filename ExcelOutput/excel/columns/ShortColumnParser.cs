using System;
using System.IO;

namespace ExcelTool
{
    public class ShortColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Short;
        }

        public string ToCSharpTypeString()
        {
            return "short";
        }

        public string ToJavaTypeString()
        {
            return "short";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (short) 0 : (short)double.Parse(value);
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write((short)double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt16()";
        }
    }
}