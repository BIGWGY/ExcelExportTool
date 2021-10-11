using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExcelTool
{
    public class LangColumnParser: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.Lang;
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