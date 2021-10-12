using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DyspaceWork
{
    public abstract class DyDataRow: IDataRow1
    {
        /// <summary>
        /// -1 为没有主键.
        /// </summary>
        /// <returns></returns>
        public virtual int GetPk()
        {
            return 0;
        }

        public virtual void ParseDataRow(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected static Dictionary<int, int> ParseIntIntDictionary(string value)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            foreach (int[] touple in value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new []{','}).Select(y => int.Parse(y)).ToArray()))
            {
                dictionary.Add(touple[0], touple[1]);
            }
            return dictionary;
        }
    }
}