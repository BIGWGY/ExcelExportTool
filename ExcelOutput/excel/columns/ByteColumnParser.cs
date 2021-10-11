using System;
using System.IO;

namespace ExcelTool
{
    public class ByteColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Byte;
        }

        public string ToCSharpTypeString()
        {
            return "byte";
        }

        public string ToJavaTypeString()
        {
            return "byte";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (short)0 : Int16.Parse(value);
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