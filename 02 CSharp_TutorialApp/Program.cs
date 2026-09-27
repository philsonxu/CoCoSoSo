using System;
using System.Windows.Forms;

namespace CSharp20Tutorial
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点，C# 2.0标准写法
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
