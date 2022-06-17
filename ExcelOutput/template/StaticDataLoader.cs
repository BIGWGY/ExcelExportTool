using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace __NAMESPACE__
{
    public class StaticDataLoader<T> where T : DataRow
    {
        // protected static string _byteDataPath = @"I:\Job\ExcelOutput\ExcelOutput\bin\Debug\client";
        protected static string _byteDataPath = Application.streamingAssetsPath + @"\table\";
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

        // 支持多平台的文本文件读取
        private static byte[] GetFileBytes(string path)
        {
            byte[] bytes = Array.Empty<byte>();
#if UNITY_ANDROID || UNITY_IOS
            bytes = GetStreamingPath(path);
#else
            bytes = FileRead(path);
#endif
            return bytes;
        }

        private static byte[] GetStreamingPath(string path)
        {
            byte[] bytes = Array.Empty<byte>();
            var uri = new Uri(path);
            var request = UnityWebRequest.Get(uri);

            var www = request.SendWebRequest();
            if (request.isNetworkError || request.isNetworkError)
            {
#if UNITY_EDITOR
                Debug.LogError("数据文件加载错误: " + path);
#endif
            }
            else
            {
                while (true)
                {
                    if (!request.isDone) continue;
                    bytes = request.downloadHandler.data;
                    break;
                }
            }

            return bytes;
        }
        
        private static byte[] FileRead(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            long n = fs.Length;
            byte[] b = new byte[n];
            int cnt, m;
            m = 0;
            cnt = fs.ReadByte();
            while (cnt != -1)
            {
                b[m++] = Convert.ToByte(cnt);
                cnt = fs.ReadByte();
            }

            return b;
        }
        
        /// <summary>
        /// 读取二进制数据.
        /// </summary>
        public static void LoadData()
        {
            string path = _byteDataPath + Path.DirectorySeparatorChar + ToUnderLine(typeof(T).Name) + ".byte";
            if (!File.Exists(path))
            {
                Debug.LogError("找不到数据配置文件: " + path);
                return;
            }
            
            byte[] bytes = GetFileBytes(path);
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader binaryReader = new BinaryReader(ms))
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
                    Debug.LogError($"数据文件 {path} 读取失败: {e.Message} ");
                    return;
                }

                Debug.LogError($"加载数据文件 {path}, 总共 {_dataList.Count} 行!");
            }
        }
        
        /// <summary>
        /// 驼峰转下滑线。
        /// </summary>
        /// <param name="camelClassName"></param>
        /// <returns></returns>
        public static string ToUnderLine(String camelClassName)
        {
            return Regex.Replace(camelClassName, "([A-Z])", "_$1").ToLower().TrimStart('_');
        }
    }
}
