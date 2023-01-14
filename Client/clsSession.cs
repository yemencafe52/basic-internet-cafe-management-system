using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal enum State : byte
    {
        CLOSED = 0,
        OPENND,
        WAITTING
    }

    internal static class Session
    {
        private static UInt64  session_no;
        private static DateTime  start;
        private static DateTime  end;
        private static double  timeleft;
        private static double  cost;
        private static State state; 
        private static int  error_no = 1;
        //===================
        private static int ticker;

        internal static void SessionInfo()
        {
            // من اخطاء النسخ واللصق ^^
            //session_no = session.session_no;
            //start = session.start;
            //end = session.end;
            //timeleft = session.timeleft;
            //cost = session.cost;
            //state = session.state;
            //error_no = session.error_no;
            //ticker = session.ticker;

        }

        internal static UInt64 Number
        {
            get
            {
                return session_no;
            }
            set
            {
                session_no = value;
            }

        }

        internal static DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }

        }

        internal static DateTime End
        {
            get
            {
                return end;
            }

        }

        internal static double TimeLeft
        {
            get
            {
                return timeleft;
            }
            set
            {
                timeleft = value;
            }

        }

        internal static double Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost = value;
            }

        }

        internal static State SessionState
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }

        }

        internal static int ErrorNumber
        {
            get
            {
                return error_no;
            }

        }
        internal static int Ticker
        {
            get
            {
                return ticker;
            }
            set
            {
                ticker = value;
            }

        }
        internal static void Reset()
        {
            session_no = 0;
            start = new DateTime();
            end = new DateTime();
            cost = 0;
            timeleft = 0;
            state = State.CLOSED;
            error_no = 1;
        }

    }

}
