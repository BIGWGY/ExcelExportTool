using System;
using System.IO;

namespace ExcelTool
{
    public class DoubleColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Double;
        }
        
        public string ToCSharpTypeString()
        {
            return "double";
        }

        public string ToJavaTypeString()
        {
            return "double";
        }

        public object ToObject(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (double) 0 : double.Parse(value);
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadDouble()";
        }
    }
}