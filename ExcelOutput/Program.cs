using System;
using System.Windows.Forms;

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
            // Console.WriteLine(JsonConvert.SerializeObject(all));
        }
    }
}
