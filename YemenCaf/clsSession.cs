using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemenCafe
{
    internal enum State:byte
    {
        CLOSED =0,
        OPENND,
        WAITTING
    }

    internal class Session
    {
        private UInt64 session_no;
        private DateTime start;
        private DateTime end;
        private double timeleft;
        private double cost;
        private State state; // we need to update our database 
        private int error_no = 1;
        //===================
        private int ticker;


        //internal Session
        //(
        //     UInt64 session_no,
        //     DateTime start,
        //     DateTime end,
        //     double timeleft,
        //     double cost,
        //     State state,
        //     int error_no ,
        //     int ticker 

        //)
        //{
        //    this.session_no = session_no;
        //    this.start = start;
        //    this.end = end;
        //    this.timeleft = timeleft;
        //    this.cost = cost;
        //    this.state = state;
        //    this.error_no = error_no;
        //    //this.ticker = ticker;
        //}


        internal Session
    (
         Session session

    )
        {

            // من اخطاء النسخ واللصق ^^
            this.session_no = session.session_no;
            this.start = session.start;
            this.end = session.end;
            this.timeleft = session.timeleft;
            this.cost = session.cost;
            this.state = session.state;
            this.error_no = session.error_no;
            this.ticker = session.ticker;
           
        }




        internal UInt64 Number
        {
            get
            {
                return this.session_no;
            }

        }

        internal DateTime Start
        {
            get
            {
                return this.start;
            }

        }

        internal DateTime End
        {
            get
            {
                return this.end;
            }

        }

        internal double TimeLeft
        {
            get
            {
                return this.timeleft;
            }
            set
            {
                this.timeleft = value;
            }

        }

        internal double Cost
        {
            get
            {
                return this.cost;
            }
            set
            {
                this.cost = value;
            }

        }

        internal State SessionState
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }

        }

        internal int ErrorNumber
        {
            get
            {
                return this.error_no;
            }

        }

        internal int Ticker
        {
            get
            {
                return this.ticker;
            }
            set
            {
                this.ticker = value;
            }

        }

        internal Session(byte station_no,UInt64 session_no)
        {
            Reset();

            if(session_no > 0)
            {
                this.session_no = session_no;
                this.start = DateTime.Now;
                this.end = DateTime.Now;
                this.ticker = Environment.TickCount;
                this.state = State.OPENND;


            }
            else
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select session_no,session_start,session_end,session_timeleft,session_cost,session_state from tblSessions where session_state <> 0 and station_no=" + station_no.ToString();

                if(db.ExcuteQuery(sql))
                {
                    if (db.DataReader.Read())
                    {
                        this.session_no = Convert.ToUInt64(db.DataReader["session_no"]);
                        this.start = Convert.ToDateTime(db.DataReader["session_start"]);
                        this.end = Convert.ToDateTime(db.DataReader["session_end"]);
                        this.timeleft = Convert.ToDouble(db.DataReader["session_timeleft"]);
                        this.cost = Convert.ToDouble(db.DataReader["session_cost"]);
                        this.state = (State)Convert.ToByte(db.DataReader["session_state"]);

                        this.ticker = Environment.TickCount;
                        this.error_no = 0;

                    }
                }
                else
                {
                    this.error_no=0;
                }

                db.CloseConnection();
            }

        }

        internal void Reset()
        {
            this.session_no = 0;
            this.start = new DateTime();
            this.end = new DateTime();
            this.cost = 0;
            this.timeleft = 0;
            this.state = State.CLOSED;
            this.error_no = 1;
        }    

    }
      
}
