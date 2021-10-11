using System;
using System.IO;
using System.Linq;

namespace ExcelTool
{
    public class StringArrayColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.StringArray;
        }

        public object ToObject(string value)
        {
            return value.Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public string ToCSharpTypeString()
        {
            return "List<string>";
        }

        public string ToJavaTypeString()
        {
            return "List<String>";
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(value);
        }

        public string ReadFromBinaryReaderExpression()
        {
            return "reader.ReadString().Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries).ToList()";
        }
    }
}