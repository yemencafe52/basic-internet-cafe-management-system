using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace YemenCafe
{
    internal static class Report
    {
        internal static DSet.tblSessionsDataTable GetSessionsInfo(DateTime from, DateTime to)
        {
            DSet.tblSessionsDataTable res = new DSet.tblSessionsDataTable();

            try
            {
                string sql = "select session_no,session_start,session_end,session_timeleft,session_cost,station_no,user_no,session_state from tblSessions where (session_state=0) and (DateValue(Format(session_end,\"yyyy/MM/dd\")) between DateValue(Format(\""+ from.ToString("yyyy/MM/dd")  +"\",\"yyyy/MM/dd\")) and DateValue(Format(\"" + to.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")))";
                using (OleDbDataAdapter ad = new OleDbDataAdapter(sql,new OleDbConnection(Constants.GetConnectionString)))
                {
                    ad.Fill(res);
                }
            }
            catch
            {

            }

            return res;
        }

        internal static DSet.tblSessionsDataTable GetSessionsInfo(DateTime from, DateTime to, Station station)
        {
            DSet.tblSessionsDataTable res = new DSet.tblSessionsDataTable();

            try
            {
                string sql = "select session_no,session_start,session_end,session_timeleft,session_cost,station_no,user_no,session_state from tblSessions where (session_state=0) and   (station_no="+ station.Number.ToString() +")  and    (DateValue(Format(session_end,\"yyyy/MM/dd\")) between DateValue(Format(\"" + from.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")) and DateValue(Format(\"" + to.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")))";
                using (OleDbDataAdapter ad = new OleDbDataAdapter(sql, new OleDbConnection(Constants.GetConnectionString)))
                {
                    ad.Fill(res);
                }
            }
            catch
            {

            }

            return res;

        }

        internal static DSet.tblSessionsDataTable GetSessionsInfo(DateTime from, DateTime to, Station station, User user)
        {

            DSet.tblSessionsDataTable res = new DSet.tblSessionsDataTable();

            try
            {
                string sql = "select session_no,session_start,session_end,session_timeleft,session_cost,station_no,user_no,session_state from tblSessions where (session_state=0) and (station_no=" + station.Number.ToString() + ")  and (user_no=" + user.Number.ToString() + ") and (DateValue(Format(session_end,\"yyyy/MM/dd\")) between DateValue(Format(\"" + from.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")) and DateValue(Format(\"" + to.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")))";
                using (OleDbDataAdapter ad = new OleDbDataAdapter(sql, new OleDbConnection(Constants.GetConnectionString)))
                {
                    ad.Fill(res);
                }
            }
            catch
            {

            }

            return res;

        }

        internal static DSet.tblSessionsDataTable GetSessionsInfo(DateTime from, DateTime to, User user)
        {

            DSet.tblSessionsDataTable res = new DSet.tblSessionsDataTable();

            try
            {
                string sql = "select session_no,session_start,session_end,session_timeleft,session_cost,station_no,user_no,session_state from tblSessions where (session_state=0) and (user_no=" + user.Number.ToString() + ") and (DateValue(Format(session_end,\"yyyy/MM/dd\")) between DateValue(Format(\"" + from.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")) and DateValue(Format(\"" + to.ToString("yyyy/MM/dd") + "\",\"yyyy/MM/dd\")))";
                using (OleDbDataAdapter ad = new OleDbDataAdapter(sql, new OleDbConnection(Constants.GetConnectionString)))
                {
                    ad.Fill(res);
                }
            }
            catch
            {

            }

            return res;

        }

    }
}
