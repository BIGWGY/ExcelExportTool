using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DyspaceWork;
using Newtonsoft.Json;

namespace ExcelTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.Run(new ControlForm());
        }

        static void TestCsByteLoad()
        {
            List<StaticVipTemplate> all = StaticDataLoader<StaticVipTemplate>.GetList();
            Console.WriteLine(JsonConvert.SerializeObject(all));
        }
    }
}
