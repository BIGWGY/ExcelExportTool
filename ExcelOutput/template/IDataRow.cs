using System.IO;

namespace __NAMESPACE__
{
    /// <summary>
    /// 本文件通过工具生成，不会修改！通过 partial 扩展本类的功能 !!!!
    /// </summary>
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