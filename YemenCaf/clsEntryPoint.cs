using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace YemenCafe
{
    static class clsEntryPoint
    {

        private static Mutex myMutex = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool has_created = false;

            myMutex = new Mutex(true, "YemenCafe", out has_created);

            if(!has_created)
            {
                MessageBox.Show("النظام يعمل حالياً");
                return;
            }

            Report.GetSessionsInfo(new DateTime(2022, 1, 1), new DateTime(2022, 12, 31));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //for(byte i=1;i<=255;i++)
            //{
            //    StationManager.AddNewStation(new Station(i, "Pc" + i, 0));
            //}

            LogManager.AddNewEvent(new Event("تشغيل النظام ...", DateTime.Now));



            frmLogin fl = new frmLogin();
            fl.ShowDialog();

            if (fl.HasLogined)
            {
                SessionManager.Init();

                Application.Run(new frmMain());

                SessionManager.Shutdown();
            }

            myMutex.Close();
            LogManager.AddNewEvent(new Event(" إيقاف النظام ...", DateTime.Now));

        }
    }
}
