// In Program.cs
using System;
using System.Windows.Forms;

namespace RDPLauncherApp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0 && args[0] == "--subdialog")
            {
                // Launch only SubDialogForm if --subdialog argument is passed
                Application.Run(new SubDialogForm());
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}
