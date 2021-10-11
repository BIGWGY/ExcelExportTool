using System;
using System.IO;

namespace ExcelTool
{
    public class FloatColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Float;
        }

        public string ToCSharpTypeString()
        {
            return "float";
        }

        public string ToJavaTypeString()
        {
            return "float";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (float) 0 : float.Parse(value);
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(float.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadSingle()";
        }
    }
}