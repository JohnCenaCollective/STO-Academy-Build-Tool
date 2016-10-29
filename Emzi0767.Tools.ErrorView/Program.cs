using System;
using System.IO;
using System.Windows.Forms;

namespace Emzi0767.Tools.ErrorView
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var args = Environment.GetCommandLineArgs();
            var path = string.Join(" ", args, 1, args.Length - 1);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(new FileInfo(path)));
        }
    }
}
