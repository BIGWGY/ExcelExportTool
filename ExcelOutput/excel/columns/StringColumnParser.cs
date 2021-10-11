using System.IO;

namespace ExcelTool
{
    public class StringColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.String;
        }

        public string ToCSharpTypeString()
        {
            return "string";
        }

        public string ToJavaTypeString()
        {
            return "String";
        }

        public object ToObject(string value)
        {
            return value;
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(value);
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadString()";
        }
    }
}