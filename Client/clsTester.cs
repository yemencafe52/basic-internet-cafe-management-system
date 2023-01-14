//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;















//while(true)
//{

//    Console.Clear();

//    if(ClientConnection.IsConnected)
//    {
//        Console.Write("CONNNECTED  :) \r\nPING : ");
//        Console.Write(ClientConnection.GetPING.ToString());

//        Console.Write("\r\n///////////////////\r\n");
//        Console.Write("Session_no=" + Session.Number.ToString() + "\r\n");
//        Console.Write("Session_start=" + Session.Start.ToString() + "\r\n");
//        Console.Write("Session_time=" + Session.TimeLeft.ToString() + "\r\n");
//        Console.Write("Session_cost=" + Session.Cost.ToString() + "\r\n");
//        Console.Write("Session_state=" + Session.SessionState.ToString() + "\r\n");

//    }
//    else

//    {
//        Console.Write("DISCONNECTED  :(   \r\n");
//    }



//    Thread.Sleep(1000);
//}

////ClientConnection.Send(new Packet(Command.PING));

//namespace Client
//{
//    //internal enum Command : byte
//    //{
//    //    PING = 1,
//    //    OPEN,
//    //    CLOSE,
//    //    UPDATE,
//    //    HndChek

//    //}
//    //internal class Packet
//    //{
//    //    private Command cmd;
//    //    private byte[] data = new byte[0];
//    //    internal Packet(Command cmd)
//    //    {
//    //        this.cmd = cmd;
//    //        this.data = new byte[0];
//    //    }
//    //    internal Packet(Command cmd, byte[] ar)
//    //    {
//    //        this.cmd = cmd;
//    //        this.data = new byte[ar.Length];
//    //        Array.Copy(ar, 0, this.data, 0, ar.Length);

//    //    }
//    //    internal Packet(byte[] ar)
//    //    {
//    //        this.cmd = (Command)ar[0];
//    //        this.data = new byte[ar.Length - 1];
//    //        Array.Copy(ar, 1, this.data, 0, ar.Length - 1);
//    //    }
//    //    internal byte[] ToBytes()
//    //    {
//    //        byte[] res = new byte[this.data.Length + 1];
//    //        res[0] = (byte)this.cmd;

//    //        Array.Copy(this.data, 0, res, 1, this.data.Length);
//    //        return res;
//    //    }
//    //    internal Command CommandInfo()
//    //    {
//    //        return this.cmd;
//    //    }

//    //    internal string DataToString()
//    //    {
//    //        return Encoding.UTF8.GetString(this.data);
//    //    }
//    //}
//    internal   class ClientConnectionT
//    {
//        private   Socket s = null;

//        private   int ticker = 0;
//        private   int ping_ticker = 0;
//        private   int ping = -1;
//        private string name;

//        private   byte[] buffer = new byte[1024 * 8];
//        private   bool is_running = false;

//        private   Thread th0;
//        internal   int GetPING
//        {
//            get
//            {

//                return ping;
//            }

//        }
//        ~ClientConnectionT()
//        {
//            Stop();
//        }

//        internal ClientConnectionT(string name)
//        {
//            this.name = name;
//            Start();
//        }
//        internal   bool Start()
//        {
//            bool res = false;

//            try
//            {
//                is_running = true;
//                th0 = new Thread(ConnctionMonitor);
//                th0.Start();
//                res = true;
//            }
//            catch
//            {
//            }

//            return res;
//        }

//        internal   bool Stop()
//        {
//            bool res = false;

//            try
//            {
//                is_running = false;
//                th0.Join();
//                CloseConnection();
//                res = true;
//            }
//            catch
//            {
//            }
//            return res;
//        }

//        internal   bool Connect()
//        {
//            bool res = false;

//            try
//            {
//                CloseConnection();
//                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                s.Connect(ParamInfo.IP, Constants.Port);
//                ticker = Environment.TickCount;


//                //Send(new Packet(Command.HndChek, Encoding.UTF8.GetBytes(Dns.GetHostName())));
//                Send(new Packet(Command.HndChek, Encoding.UTF8.GetBytes(this.name)));
//                s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recive), null);
//                res = true;
//            }
//            catch
//            {
//                CloseConnection();
//            }

//            return res;


//        }

//        internal   void CloseConnection()
//        {
//            try
//            {
//                if (s != null)
//                {
//                    if (s.Connected)
//                    {
//                        s.Shutdown(SocketShutdown.Both);
//                        s.Disconnect(false);
//                    }

//                    s.Close();
//                    s.Dispose();
//                    s = null;
//                }

//                ping = -1;

//            }
//            catch
//            {
//                s = null;
//            }

//        }

//        internal   bool Send(Packet packet)
//        {
//            bool res = false;

//            try
//            {
//                if (IsConnected)
//                {
//                    s.Send(packet.ToBytes());
//                    res = true;
//                }

//            }
//            catch
//            {
//                CloseConnection();
//            }


//            return res;
//        }

//        internal   void Recive(IAsyncResult ar)
//        {
//            try
//            {
//                int len = s.EndReceive(ar);

//                if (len > 0)
//                {
//                    ticker = Environment.TickCount;
//                    byte[] tmp = new byte[len];
//                    Array.Copy(buffer, 0, tmp, 0, len);

//                    Packet packet = new Packet(tmp);

//                    switch (packet.CommandInfo())
//                    {
//                        case Command.UPDATE:
//                            {
//                                string data = packet.DataToString();
//                                string[] info = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

//                                if (info.Count() != 5)
//                                {
//                                    throw new Exception();
//                                }

//                                Session.Number = UInt64.Parse(info[0]);
//                                Session.Start = new DateTime(long.Parse(info[1]));
//                                Session.TimeLeft = double.Parse(info[2]);
//                                Session.Cost = double.Parse(info[3]);
//                                Session.SessionState = (State)(byte.Parse(info[4]));

//                                break;
//                            }
//                        case Command.PING:
//                            {

//                                ping = Environment.TickCount - ping_ticker;

//                                break;
//                            }
//                    }

//                    s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recive), null);
//                }
//                else
//                {
//                    CloseConnection();
//                }
//            }
//            catch
//            {
//                CloseConnection();
//            }
//        }

//        internal   void ConnctionMonitor()
//        {
//            while (is_running)
//            {
//                if (!IsConnected)
//                {
//                    Connect();
//                }
//                else
//                {
//                    ping_ticker = Environment.TickCount;
//                    Send(new Packet(Command.PING));
//                }

//                Thread.Sleep(3000);
//            }
//        }
//        internal   bool IsConnected
//        {
//            get
//            {
//                bool res = false;

//                if (s != null)
//                {
//                    if (s.Connected)
//                    {
//                        if ((Environment.TickCount - ticker) < 1000 * 15)
//                        {
//                            res = true;
//                        }
//                    }
//                }

//                return res;
//            }
//        }

//    }
//}
