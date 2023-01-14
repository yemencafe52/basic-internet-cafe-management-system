using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YemenCafe//e
{
    internal static class ParamInfo
    {
        private static double unit_per_second = 60;
        private static double price_per_unit = 3;
        private static string backupPath = "";
        private static byte station_count = 10;

        internal static bool UpdateParamInfo(
          double unit_per_second,
          double price_per_unit,
          string backupPatp,
          byte station_count
        )

        {
            bool res = false;

            try
            {

                if(!CanUpdate())
                {
                    throw new Exception();
                }

                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "update tblParams set p_ups=" + unit_per_second + ",p_ppu=" + price_per_unit + ",p_dbpu='" + backupPatp + "',p_sc=" + station_count;

                if (db.ExcuteNonQuery(sql) == 1)
                {
                    res = true;
                }

                LogManager.AddNewEvent(new Event("unit="+ unit_per_second +",price="+ price_per_unit +",count="+ station_count +"  تم تعديل الاعدادات  ...", DateTime.Now));
            }
            catch
            {

            }

            return res;
        }

        internal static bool GetParamInfo()
        {
            bool res = false;
            try
            {

                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select p_ups,p_ppu,p_dbpu,p_sc from tblParams";

                if (db.ExcuteQuery(sql))
                {
                    if (db.DataReader.Read())
                    {
                        unit_per_second = Convert.ToDouble(db.DataReader["p_ups"]);
                        price_per_unit = Convert.ToDouble(db.DataReader["p_ppu"]);
                        backupPath = Convert.ToString(db.DataReader["p_dbpu"]);
                        station_count = Convert.ToByte(db.DataReader["p_sc"]);

                    }
                }

                db.CloseConnection();
                res = true;

            }
            catch
            {


            }
            return res;

        }

        internal static double GetUnitPerSecond
        {
            get
            {
                return unit_per_second;
            }
        }

        internal static double PricePerUnit
        {
            get
            {
                return price_per_unit;
            }
        }


        private static bool CanUpdate()
        {
            bool res = false;

            try
            {
                foreach(Station s in SessionManager.GetStations)
                {
                    if(s.SessionInfo.SessionState != State.CLOSED)
                    {
                        return res;
                    }
                }

                res = true;

            }
            catch
            {

            }

            return res;
                 
        }
        internal static byte StationsCount
        {
            get
            {
                return station_count;
            }
        }
    }
}
