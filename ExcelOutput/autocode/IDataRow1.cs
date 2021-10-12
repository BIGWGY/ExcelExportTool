using System.IO;

namespace DyspaceWork
{
    public interface IDataRow1
    {
        /// <summary>
        /// 主键。
        /// </summary>
        /// <returns></returns>
        int GetPk();

        /// <summary>
        /// 读取行数据。
        /// </summary>
        /// <param name="reader"></param>
        void ParseDataRow(BinaryReader reader);
    }
}