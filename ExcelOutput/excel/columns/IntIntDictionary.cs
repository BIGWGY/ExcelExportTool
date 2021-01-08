using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelTool
{
    public class IntIntDictionary: IColumnParser
    {
        public ColumnType GetColumnType()
        {
            return ColumnType.IntIntDictionary;
        }

        public object ToObject(string value)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            foreach (int[] touple in value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new []{','}).Select(y => int.Parse(y)).ToArray()))
            {
                dictionary.Add(touple[0], touple[1]);
            }
            return dictionary;
        }

        public string ToCSharpTypeString()
        {
            return "Dictionary<int, int>";
        }

        public void WriteToBinaryWriter(BinaryWriter writer, string value)
        {
            writer.Write(value);
        }

        public string ReadFromBinaryReaderExpression()
        {
            return "ParseIntIntDictionary(reader.ReadString())";
        }
    }
}