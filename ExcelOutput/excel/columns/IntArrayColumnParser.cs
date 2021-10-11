using System;
using System.IO;
using System.Linq;

namespace ExcelTool
{
    public class IntArrayColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.IntArray;
        }

        public object ToObject(string value)
        {
            return value.Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
        }

        public string ToJavaTypeString()
        {
            return "List<Integer>";
        }

        public string ToCSharpTypeString()
        {
            return "List<int>";
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(value);
        }

        public string ReadFromBinaryReaderExpression()
        {
            return "reader.ReadString().Split(new []{'|'}, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList()";
        }
    }
}