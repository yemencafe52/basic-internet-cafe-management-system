using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace YemenCafe
{
    internal class Station
    {
        private byte station_no;
        private string station_name;
        private Session session;
        private int error_no = 1;
        //=======================
        private byte[] data = new byte[1024 * 8];
        private Socket s = null;

        internal Station(byte number)
        {

            try
            {
                AccessDB db = new AccessDB(Constants.GetConnectionString);
                string sql = "select station_no,station_name from tblStations where station_no=" + number.ToString();
                //string sql = "select station_no,station_";

                if (db.ExcuteQuery(sql))
                {
                    if (db.DataReader.Read())
                    {
                        this.station_no = Convert.ToByte(db.DataReader["station_no"]);
                        this.station_name = Convert.ToString(db.DataReader["station_name"]);
                        this.session = new Session(this.station_no, 0);

                        if (this.session.ErrorNumber == 0)
                        {
                            this.error_no = 0;
                        }
                    }

                }
                db.CloseConnection();
                
            }
            catch
            {
                error_no = 1;
            }

            //this.station_no = 1;
            //this.station_name = "Pc01";

            //this.session = new Session(this.station_no, 0);

            //if (this.SessionInfo.ErrorNumber == 0)
            //{
            //    this.error_no = 0;
            //}
        }

        internal Station(
               byte station_no,
               string station_name,
               int error_no
        )
        {
            this.station_no = station_no;
            this.station_name = station_name;
            this.error_no = error_no;
            this.session = new Session(this.station_no, 0);

        }

        internal byte Number
        {
            get
            {
                return this.station_no;
            }
        }

        internal string Name
        {
            get
            {
                return this.station_name;
            }
        }

        internal int ErrorNumber
        {
            get
            {
                return this.error_no;
            }
        }



        internal Session SessionInfo
        {
            get
            {
                return this.session;
            }
            set
            {
                this.session = value;
            }
        }

        internal Socket SocketInfo
        {
            set
            {
                this.s = value;
                try
                {
                    this.s.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnRecive), s);
                }
                catch
                {
                    this.s = null;
                }
            }
        }

        internal bool Send(Packet packet)
        {
            bool res = false;

            try
            {
                if (s != null)
                {
                    if (s.Connected)
                    {
                        s.Send(packet.ToBytes());
                        res = true;
                    }
                }
            }
            catch
            {
                CloseConnection();
            }

            return res;
        }

        private void OnRecive(IAsyncResult ar)
        {
            try
            {
                int len = this.s.EndReceive(ar);

                if (len > 0)
                {
                    byte[] tmp = new byte[len];
                    Array.Copy(data, 0, tmp, 0, len);

                    Packet packet = new Packet(tmp);

                    switch (packet.CommandInfo())
                    {
                        case Command.PING:
                            {
                                Send(new Packet(Command.PING));
                                break;
                            }

                        case Command.OPEN:
                            {
                                Station s = SessionManager.GetStations.Find(p => p.Name == this.Name);
                                SessionManager.OpenNewSession(s);
                                break;
                            }

                        case Command.CLOSE:
                            {
                                Station s = SessionManager.GetStations.Find(p => p.Name == this.Name);
                                SessionManager.CloseSession(s);
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
                this.s.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnRecive), s);

            }
            catch
            {
                CloseConnection();
            }

           
        }

        internal void CloseConnection()
        {
            try
            {
                if(s != null)
                {
                    if (s.Connected)
                    {
                        s.Shutdown(SocketShutdown.Both);
                        s.Disconnect(false);
                    }

                    s.Close();
                    s.Dispose();
                    s = null;
                }
            }
            catch
            {
                s = null;
            }
        }

        internal bool IsConnected
        {
            get
            {
                bool res = false;
                if(s != null)
                {
                    if (s.Connected)
                    {
                        res = true;
                    }
                }
               
                return res;
            }
        }
    }
    internal static class StationManager
    {
        internal static bool AddNewStation(Station station)
        {
            bool res = false;

            try
            {
                res = true;
            }
            catch
            {

            }

            return res;
        }
    }
}
