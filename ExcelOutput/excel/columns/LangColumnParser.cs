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
            return "Int32";
        }

        public object ToObject(string value)
        {
            return value;
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(value.Equals("") ? (Int32)0 : Int32.Parse(value));
        }

        public string ReadFromBinaryReaderExpression()
        {
            return @"reader.ReadInt32()";
        }
    }
}