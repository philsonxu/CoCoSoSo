using System;
using System.Windows.Forms;
using CSharp20AIFull.UI;

namespace CSharp20AIFull
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
