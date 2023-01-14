using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Threading;
using System.IO;

namespace Client
{
    static class EntryPoint
    {
        private static Mutex myMutex = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool has_created = false;
                myMutex = new Mutex(true, "ycclient", out has_created);

                if (!has_created)
                {
                    //MessageBox.Show("النظام يعمل حالياً");
                    return;
                }

                if (!Utilites.CanRWPI())
                {
                    myMutex.Close();
                    MessageBox.Show("يجب تشغيل النظام كمسؤول");
                    return;
                }

                if (Application.ExecutablePath != Constants.SystemPath)
                {
                    if (!Utilites.Install())
                    {
                        MessageBox.Show("تعذر عملية التثبيت.");
                        myMutex.Close();
                        return;
                    }
                }

                if (File.Exists(Constants.ConfigPath))
                {
                    if (!ParamInfo.ReadConfig())
                    {
                        MessageBox.Show("تعذر قراءة الاعدادات .");
                        myMutex.Close();
                        return;
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    ClientConnection.Start();

                    frmMain fm = new frmMain();
                    Application.Run(fm);

                    ClientConnection.Stop();

                    if (fm.CloseToSetup)
                    {
                        frmSettings fs = new frmSettings();
                        fs.ShowDialog();

                    }
                }
                else
                {
                    frmSettings fs = new frmSettings();
                    fs.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show(":(  اوبس , خطأ فادح .");
            }
        }
    }
}
