using System.IO;

namespace __NAMESPACE__
{
    public interface IDataRow
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
        void ReadRowData(BinaryReader reader);
    }
}