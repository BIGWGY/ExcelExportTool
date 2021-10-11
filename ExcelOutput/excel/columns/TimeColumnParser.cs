using System;
using System.IO;

namespace ExcelTool
{
    public class TimeColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Time;
        }

        public string ToCSharpTypeString()
        {
            return "DateTime";
        }

        public string ToJavaTypeString()
        {
            return "String";
        }

        public object ToObject(string value)
        {
            return DateTime.FromOADate(double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value)).ToString();
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(DateTime.FromOADate(double.Parse(string.IsNullOrWhiteSpace(value) ? "0" : value)).Ticks);
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"new DateTime(reader.ReadInt64())";
        }
    }
}