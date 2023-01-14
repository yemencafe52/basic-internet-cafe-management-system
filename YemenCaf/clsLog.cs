using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YemenCafe
{
    internal class Event
    {
        private string msg;
        private DateTime time;

        internal Event()
        {
            this.msg = "";
            this.time = new DateTime();
        }

        internal Event(string msg,DateTime time)
        {
            this.msg = msg;
            this.time = time;
        }


        internal string GetMessage
        {
            get
            {
                return this.msg;
            }
        }

        internal DateTime Time
        {
            get
            {
                return this.time;
            }
        }
    }
    internal static class LogManager
    {
        private static List<Event> events = new List<Event>();
        private static object mylocker2 = new object();

        internal static bool AddNewEvent(Event e)
        {
            lock (mylocker2)
            {
                bool res = false;

                try
                {
                    events.Add(e);

                    try
                    {
                        if (!(Directory.Exists(Constants.GetLogPath)))
                        {
                            Directory.CreateDirectory(Constants.GetLogPath);
                        }

                        string eventText = "[" + e.Time.ToString() + "] : " + e.GetMessage + ". \r\n";
                        File.AppendAllText(Constants.GetLogPath + @"\YC@" + DateTime.Now.ToString("yyyy-MM-dd"), eventText);

                    }
                    catch
                    {

                    }


                    res = true;

                }
                catch
                {

                }

                return res;
            }
        }

        internal static bool ReadEvent(ref Event e)
        {
            bool res = false;

            try
            {
                if(events.Count > 0)
                {
                    e = events[0];
                    events.RemoveAt(0);
                    res = true;
                }
            }
            catch
            {

            }

            return  res;
        }
    }
}
