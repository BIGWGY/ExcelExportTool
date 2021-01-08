using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelTool;

namespace DyspaceWork
{
    public class StaticDataLoader<T> where T: DataRow
    {
        private const string _byteDataPath = @"I:\Job\ExcelOutput\ExcelOutput\bin\Debug\client";
        // private static string _byteDataPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "example";

        /// <summary>
        /// 获取数据.
        /// </summary>
        private static List<T> _dataList = new List<T>();
        
        /// <summary>
        /// 有主键的数据才会有.
        /// </summary>
        private static Dictionary<int, T> _dataDictionary = new Dictionary<int, T>();

        static StaticDataLoader()
        {
            LoadData();
        }

        /// <summary>
        /// 获取一条数据。
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static T GetData(int pk)
        {
            if (!_dataDictionary.ContainsKey(pk))
            {
                return null;
            }

            return _dataDictionary[pk];
        }

        /// <summary>
        /// 返回所有数据。
        /// </summary>
        /// <returns></returns>
        public static List<T> GetList()
        {
            return _dataList;
        }

        /// <summary>
        /// 读取二进制数据.
        /// </summary>
        public static void LoadData()
        {
            string path = _byteDataPath + Path.DirectorySeparatorChar + StringUtils.ToUnderLine(typeof(T).Name) + ".byte";
            if (!File.Exists(path))
            {
                Console.WriteLine("找不到数据配置文件: " + path);
                return;
            }
            
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {

                long length = binaryReader.BaseStream.Length;
                try
                {
                    while (binaryReader.BaseStream.Position < length)
                    {
                        T t = Activator.CreateInstance<T>();
                        t.ReadRowData(binaryReader);
                        _dataList.Add(t);
                        if (t.GetPk() > 0)
                        {
                            _dataDictionary.Add(t.GetPk(), t);    
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("数据文件 {0} 读取失败: {1} " , path, e.Message);
                    throw;
                }
                
                Console.WriteLine($"加载数据文件 {path}, 总共 {_dataList.Count} 行!");
            }
        }
    }
}