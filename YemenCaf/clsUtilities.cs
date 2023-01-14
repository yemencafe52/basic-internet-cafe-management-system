using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace YemenCafe
{
    internal static class Utilities
    {
        internal static bool TestDb()
        {
            bool res = false;

            try
            {
                if (CheckDataBaseFile())
                {
                    AccessDB db = new AccessDB(Constants.GetConnectionString);
                    string sql = "select count(*) from tblUsers";
                    if (db.ExcuteQuery(sql))
                    {
                        res = true;
                    }
                    db.CloseConnection();
                }
            }
            catch
            {

            }

            return res;
        }

        internal static bool CheckDataBaseFile()
        {
            bool res = false;

            try
            {
               if(File.Exists(Constants.GetDbPath))
                {
                    res = true;
                }
            }
            catch
            {

            }

            return res;
        }

        internal static bool BackupDB(string path)
        {
            bool res = false;

            try
            {
                File.Copy(Constants.GetDbPath, path, false);
                res = true;
            }
            catch
            {

            }

            return res;
        }
    }
}
