
using System.Collections.Generic;

namespace ExcelTool
{
    public class Word
    {
        private static List<string> _words = new List<string>()
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "ecent",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "get",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "out",
            "override",
            "partial",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "set",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "value",
            "virtual",
            "volatile",
            "volatile",
            "void",
            "where",
            "while",
            "yield",
        };
        
        /// <summary>
        /// 是否为c#关键字。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsCSharpKeyWord(string word)
        {
            return _words.Contains(word);
        }
    }
}