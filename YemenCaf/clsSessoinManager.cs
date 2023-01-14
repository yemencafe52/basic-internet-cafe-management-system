using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace YemenCafe
{
    internal static class SessionManager
    {
        private static List<Station> stations = new List<Station>();
        private static bool is_server_active = false;
        private static Thread th0;

        private static object s_locker = new object();
        private static UInt64 sid = 0;

        internal static List<Station> GetStations
        {
            get

            {
                return stations;
            }

        }

        internal static bool OpenNewSession(Station station)
        {
            lock (s_locker)
            {

                bool res = false;

                try
                {

                    if(station.SessionInfo.SessionState != State.CLOSED)
                    {
                        throw new Exception("");
                    }

                    UInt64 session_no = GenereateNewSessionID();

                    if(session_no == 0)
                    {
                        throw new Exception("");
                    }

                    station.SessionInfo = new Session(station.Number, session_no);
                    // ماذا نسينا؟ ... حالة الجلسة  


                    UpdateSessionInfo(station);
                    string sql = "insert into tblSessions (session_no,session_start,session_end,session_timeleft,session_cost,user_no,station_no,session_state) values("+ station.SessionInfo.Number.ToString() +",'"+ station.SessionInfo.Start.ToString()+ "','"+ station.SessionInfo.End.ToString() + "',0,0,"+ UserManager.GetActiveUser.Number.ToString() +"," + station.Number.ToString()+ ","+ ((byte)station.SessionInfo.SessionState).ToString() + ")";

                    AccessDB db = new AccessDB(Constants.GetConnectionString);

                    if((db.ExcuteNonQuery(sql) != 1))
                    {
                        throw new Exception("");
                    }

                    LogManager.AddNewEvent(new Event(" فتح جلسة" + station.Name ,  DateTime.Now));
                    res = true;
                }
                catch
                {

                }

                return res;
            }
        }

        internal static bool CloseSession(Station station)
        {
            lock (s_locker)
            {

                bool res = false;

                try
                {

                    if (station.SessionInfo.SessionState != State.OPENND)
                    {
                        throw new Exception("");
                    }

                    if (!(UpdateSessionInfo(station)))
                    {

                        throw new Exception("");
                    }

                    station.SessionInfo.SessionState = State.WAITTING;

                    if (!(SaveSessionToDB(station)))
                    {
                        station.SessionInfo.SessionState = State.OPENND;
                        throw new Exception("");
                    }

                    LogManager.AddNewEvent(new Event(" اغلاق الجلسة" + station.Name, DateTime.Now));
                    res = true;
                }
                catch 
                {

                }

                return res;
            }
        }


        internal static bool EndSession(Station station)
        {
            lock (s_locker)
            {

                bool res = false;

                try
                {

                     if (station.SessionInfo.SessionState != State.WAITTING)
                    {
                        throw new Exception("");
                    }

                    if (!(UpdateSessionInfo(station)))
                    {

                        throw new Exception("");
                    }

                    station.SessionInfo.SessionState = State.CLOSED;

                    if (!(SaveSessionToDB(station)))
                    {
                        station.SessionInfo.SessionState = State.WAITTING;
                        throw new Exception("");
                    }

                    station.SessionInfo.Reset();
                    LogManager.AddNewEvent(new Event(" إنهاء الجلسة" + station.Name, DateTime.Now));
                    res = true;
                }
                catch
                {

                }

                return res;
            }
        }

        internal static UInt64 GenereateNewSessionID()
        {
            lock (s_locker)
            {
                if (sid > 0)
                {
                    return ++sid;
                }

                try
                {

                    AccessDB db = new AccessDB(Constants.GetConnectionString);
                    string sql = "select max(session_no) as res from tblSessions";

                    if(db.ExcuteQuery(sql))
                    {
                        if(db.DataReader.Read())
                        {
                            string res = db.DataReader["res"].ToString();
                            if(UInt64.TryParse(res, out sid))
                            {
                                sid++;
                            }
                            else
                            {
                                sid = 1;
                            }
                        }
                    }

                    db.CloseConnection();

                }
                catch
                {    
                }

            }

            return sid;
        }

        internal static bool SaveSessionToDB(Station station)
        {
            lock (s_locker)
            {
                bool res = false;

                try
                {
                    AccessDB db = new AccessDB(Constants.GetConnectionString);
                    string sql = "update tblSessions set session_end='" + station.SessionInfo.End.ToString() + "',session_timeleft=" + station.SessionInfo.TimeLeft.ToString() + ",session_cost=" + station.SessionInfo.Cost.ToString() + ",user_no=" + UserManager.GetActiveUser.Number.ToString() + ",session_state="+ ((byte)station.SessionInfo.SessionState).ToString() +" where session_no=" + station.SessionInfo.Number.ToString();

                    if (db.ExcuteNonQuery(sql) == 1)
                    {
                        res = true;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                catch
                {
                    //LogManager.AddNewEvent(new Event(" session_no=" ",station=  تعذر حفظ الجسلة في قاعدة البيانات  ", DateTime.Now));
                }

                return res;
            }
        }

        internal static bool UpdateSessionInfo(Station station)
        {
            lock(s_locker)
            {
                bool res = false;
                try
                {
                    int diff = Environment.TickCount - station.SessionInfo.Ticker;
                    station.SessionInfo.Ticker = Environment.TickCount;

                    double sec = ((double)diff / (double)1000);
                    station.SessionInfo.TimeLeft += sec;
                    station.SessionInfo.Cost = Math.Round((((int)(station.SessionInfo.TimeLeft / ParamInfo.GetUnitPerSecond))+1) * ParamInfo.PricePerUnit,2);
                    //station.SessionInfo.Cost = Math.Round((station.SessionInfo.TimeLeft / ParamInfo.GetUnitPerSecond) * ParamInfo.PricePerUnit, 2);

                    res = true;

                }
                catch
                {

                }

                return res;
            }
            
        }

        internal static bool SendSessionInfo(Station s)
        {
            lock (s_locker)
            {
                bool res = false;

                try
                {

                    if (s.IsConnected)
                    {
                        string data = "";

                        data += s.SessionInfo.Number.ToString();
                        data += ";";

                        data += s.SessionInfo.Start.Ticks.ToString();
                        data += ";";

                        data += s.SessionInfo.TimeLeft.ToString();
                        data += ";";

                        data += s.SessionInfo.Cost.ToString();
                        data += ";";

                        data += ((byte)s.SessionInfo.SessionState).ToString();

                        s.Send(new Packet(Command.UPDATE, Encoding.UTF8.GetBytes(data)));
                        res = true;
                    }
                }
                catch
                {

                }

                return res;

            }
        }

            internal static bool Swap(Station from,Station to)
        {
            lock (s_locker)
            {
                bool res = false;

                try
                {
                    if (from.SessionInfo.SessionState != State.OPENND)
                    {
                        throw new Exception();
                    }

                    if (to.SessionInfo.SessionState != State.CLOSED)
                    {
                        throw new Exception();
                    }

                    to.SessionInfo = new Session(from.SessionInfo);
                    //to.SessionInfo = new Session(from.SessionInfo.Number, from.SessionInfo.Start, from.SessionInfo.End, from.SessionInfo.TimeLeft, from.SessionInfo.Cost, from.SessionInfo.SessionState, from.SessionInfo.ErrorNumber, from.);
                    AccessDB db = new AccessDB(Constants.GetConnectionString);
                    string sql = "update tblSessions set station_no=" + to.Number + " where session_no=" + from.SessionInfo.Number;

                    if (db.ExcuteNonQuery(sql) == 1)
                    {
                        from.SessionInfo.Reset();
                        res = true;

                        LogManager.AddNewEvent(new Event(" نقل العميل من المحطة  " +  from.Name + " الى المحطة " + to.Name  + " " , DateTime.Now));
                    }
                    else
                    {
                        throw new Exception();
                    }


                }
                catch
                {
                    LogManager.AddNewEvent(new Event(" نقل العميل من المحطة تعذر  " + from.Name + " الى المحطة " + to.Name + " ", DateTime.Now));
                }

                return res;
            }
        }

        internal static bool Init()
        {
            bool res = false;

            try
            {
                if (!(ParamInfo.GetParamInfo()))
                {
                    throw new Exception();
                }

                if(!(Server.Start()))
                {
                    throw new Exception();
                }

                stations.Clear();

                for (byte i= 1; i <=ParamInfo.StationsCount;i++)
                {
                    Station s = new Station(i);

                    if(s.ErrorNumber != 0)
                    {
                        throw new Exception("");
                    }

                    stations.Add(s);
                }

                th0 = new Thread(SessionsMonitor);
                is_server_active = true;
                th0.Start();

                res = true;

            }
            catch
            {
                LogManager.AddNewEvent(new Event("تعذر بداء تشغيل النظام", DateTime.Now));
                stations.Clear();
            }

            return res;
        }

        internal static bool Shutdown()
        {
            bool res = true;
            try
            {
                is_server_active = false;
                th0.Join();

                Server.Stop();

                while (stations.Count>0)
                {
                    //stations[0].Send(new Packet(Command.CLOSE));


                    if(stations[0].SessionInfo.SessionState != State.CLOSED)
                    {
                        if (SaveSessionToDB(stations[0]) == false)
                        {
                            if(res)
                            {
                                res = false;
                            }
                        }
                    }

                    stations.RemoveAt(0);
                }

                if(!res)
                {
                    throw new Exception();
                }

            }
            catch
            {
                LogManager.AddNewEvent(new Event(" تعذر ايقاف النظام بصورة صحيحة ", DateTime.Now));
            }

            return res;
        }
        private static void SessionsMonitor()
        {
            while(true)
            {
                for(byte index = 0;index < stations.Count;index++)
                {
                    if (is_server_active == false)
                    {
                        return;
                    }

                    if (stations[index].SessionInfo.SessionState != State.CLOSED)
                    {
                        if (stations[index].SessionInfo.SessionState == State.OPENND)
                        {
                            UpdateSessionInfo(stations[index]);
                        }
                        else
                        {
                            //////////////////
                        }
                        Thread.Sleep(5);
                    }

                    SendSessionInfo(stations[index]);

                }

                Thread.Sleep(1000);
            }
        }

    }
}
