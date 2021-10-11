using System;
using System.IO;

namespace ExcelTool
{
    public interface IColumnParser
    {
        /// <summary>
        /// 解析的数据类型。
        /// </summary>
        /// <returns></returns>
        ColumnType GetColumnType();

        /// <summary>
        /// json 格式。
        /// </summary>
        /// <returns></returns>
        Object ToObject(string value);

        /// <summary>
        /// 返回 csharp 对应的数据类型。
        /// </summary>
        /// <returns></returns>
        string ToCSharpTypeString();
    
        /// <summary>
        /// 返回 java 对应的数据类型。
        /// </summary>
        /// <returns></returns>
        string ToJavaTypeString();
        
        /// <summary>
        /// 写入二进制流。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        void WriteToBinaryWriter(BinaryWriter writer, string value);

        /// <summary>
        /// 读取数据的表达式。
        /// </summary>
        /// <returns></returns>
        string ReadFromBinaryReaderExpression();
    }
}