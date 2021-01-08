/*********************************************************************
* @link https://blog.csdn.net/xiaokui604/article/details/7496737
**********************************************************************/

using System.Runtime.InteropServices;
using System.Text;

namespace ExcelTool
{
    public class IniFile
        {
            /// <summary>
            ///  INI文件路径
            /// </summary>
            public string path;            
         
            //声明写INI文件的API函数
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
         
            //声明读INI文件的API函数
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
 
            //类的构造函数，传递INI文件的路径和文件名
            public IniFile(string INIPath) 
            { 
                path = INIPath; 
            } 
 
            //写INI文件
            public void IniWriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, path);
            }
 
            //读取INI文件
            public string IniReadValue(string Section, string Key, string defaultValue ="")
            {
                StringBuilder temp = new StringBuilder(255);
                GetPrivateProfileString(Section, Key, "", temp, 255, path);
                return temp.Length <= 0 ? defaultValue : temp.ToString();
            }

            public void TestIniFileRead()
            {
                IniFile ini = new IniFile("D://config.ini"); 
                string BucketName = ini.IniReadValue("operatorinformation","bucket");
                string OperatorName = ini.IniReadValue("operatorinformation", "operatorname");
                string OperatorPwd = ini.IniReadValue("operatorinformation", "operatorpwd");
            }

            public void TestInitFileWrite()
            {
                IniFile ini = new IniFile("D://config.ini"); 
                ini.IniWriteValue("operatorinformation", "bucket", "BucketName");
                ini.IniWriteValue("operatorinformation", "operatorname", "OperatorName");
                ini.IniWriteValue("operatorinformation", "operatorpwd", "OperatorPwd");
            }
        } 
}