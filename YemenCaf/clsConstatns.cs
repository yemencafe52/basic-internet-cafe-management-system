using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YemenCafe
{
    internal static class Constants
    {
        private static string dbPath = Application.StartupPath + @"\DataBase\db.accdb";
        private static string connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath + @";Persist Security Info=False;";

        private static string logPath = Application.StartupPath + @"\Log";

        internal static string GetConnectionString
        {
            get
            {
                return connection_string;
            }
        }

        internal static string GetDbPath
        {
            get
            {
                return dbPath;
            }
        }

        internal static string GetLogPath
        {
            get
            {
                return logPath;
            }
        }


    }
}
