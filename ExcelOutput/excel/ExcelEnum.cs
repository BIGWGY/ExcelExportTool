using System;
using System.Collections.Generic;

namespace ExcelTool
{
    public class ExcelEnum
    {
        /// <summary>
        /// 枚举类所在的文件路径。
        /// </summary>
        private string _filepath;

        /// <summary>
        /// 枚举类型的名字。
        /// </summary>
        private string _enumClassName;

        /// <summary>
        /// 枚举继承的类型。
        /// </summary>
        private string _heritType = "";

        /// <summary>
        /// 枚举值列表。
        /// </summary>
        private List<EnumInfo> _enumInfos = new List<EnumInfo>();

        public string HeritType
        {
            get => _heritType;
            set => _heritType = value;
        }

        public string Filepath
        {
            get => _filepath;
            set => _filepath = value;
        }

        public string EnumClassName
        {
            get => _enumClassName;
            set => _enumClassName = value;
        }

        /// <summary>
        /// 枚举的最大值。
        /// </summary>
        public int EnumMaxValue
        {
            get
            {
                int max = -1;
                foreach (var info in _enumInfos)
                {
                    max = Math.Max(info.Value, max);
                }

                return Math.Max(max, 0);
            }
        }

        public List<EnumInfo> EnumInfos
        {
            get => _enumInfos;
            set => _enumInfos = value;
        }

        /// <summary>
        /// 是否存在枚举值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HasEnumValue(int value)
        {
            foreach (var enumInfo in _enumInfos)
            {
                if (enumInfo.Value == value)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在枚举名字。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasEnumName(string name)
        {
            foreach (var enumInfo in _enumInfos)
            {
                if (enumInfo.Name == name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}