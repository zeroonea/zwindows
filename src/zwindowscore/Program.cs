using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using CenterTaskbar;
using Microsoft.Win32;

namespace zwindowscore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Only allow one instance of this application to run at a time using GUID
            var assyGuid = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value.ToUpper();
            using (new Mutex(true, assyGuid, out var firstInstance))
            {
                if (!firstInstance)
                {
                    MessageBox.Show("Another instance is already running.", "CenterTaskbar", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Settings());
                //Application.Run(new TrayApplication(args));
            }
        }
    }
}
