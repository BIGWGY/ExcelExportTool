using System.Text.RegularExpressions;
using DyspaceWork;
using ikvm.extensions;
using java.lang;

namespace ExcelTool
{
    public class StringUtils
    {
        /// <summary>
        /// 下划线转换为驼峰。
        /// </summary>
        /// <param name="underlineWord"></param>
        /// <returns></returns>
        public static string ToCamel(string underlineWord)
        {
            StringBuffer camel = new StringBuffer();
            string[] list = underlineWord.split("_");
            foreach (var s in list)
            {
                camel.append(s.substring(0, 1).toUpperCase() + s.substring(1));
            }

            return camel.toString();
        }

        /// <summary>
        /// 驼峰转下滑线。
        /// </summary>
        /// <param name="camelClassName"></param>
        /// <returns></returns>
        public static string ToUnderLine(string camelClassName)
        {
            return camelClassName.replaceAll("([A-Z])", "_$1").toLowerCase().TrimStart('_');
        }
    }
}