using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace YemenCafe
{
    internal enum Command : byte
    {
        PING =1,
        OPEN,
        CLOSE,
        UPDATE,
        HndChek

    }

    internal class Packet
    {
        private Command cmd;
        private byte[] data;

        internal Packet(Command cmd)
        {
            this.cmd = cmd;
            this.data = new byte[0];
        }

        internal Packet(Command cmd,byte[] ar)
        {
            this.cmd = cmd;
            this.data = new byte[ar.Length];
            Array.Copy(ar, 0, this.data, 0, ar.Length);

        }

        internal Packet(byte[] ar)
        {
            this.cmd = (Command)ar[0];
            this.data = new byte[ar.Length - 1];
            Array.Copy(ar, 1, this.data, 0, ar.Length - 1);
        }

        internal byte[] ToBytes()
        {
            byte[] res = new byte[this.data.Length + 1];
            res[0] = (byte)this.cmd;

            Array.Copy(this.data, 0, res, 1, this.data.Length);
            return res;
        }

        internal Command CommandInfo()
        {
            return this.cmd;
        }

        internal string DataToString()
        {
            return Encoding.UTF8.GetString(this.data);
        }
    }
    internal static class Server
    {
        private static int port = 967;
        private static Socket server;
        private static bool is_running = false;

        internal static bool Start()
        {
            bool res = false;
            try
            {
                if(server != null)
                {
                    return res;
                }

                IPEndPoint iep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(iep);
                server.Listen(0);  // غير محدود 
                server.BeginAccept(new AsyncCallback(OnAccept), server);
                is_running = true;
                res = true;
            }
            catch
            {
                is_running = false;
                server = null;
            }
            return res;
        }

        internal static bool Stop()
        {
            bool res = false;
            try
            {
                if(server !=null)
                {
                    return res;
                }

                server.Close();
                is_running = false;
                res = true;
            }

            catch
            {

            }
            return res;
        }

        private static void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket s = server.EndAccept(ar);
                server.BeginAccept(new AsyncCallback(OnAccept), server);

                try
                {
                    byte[] data = new byte[1024 * 8];
                    s.ReceiveTimeout = 3000;
                    int len = s.Receive(data, data.Length, SocketFlags.None);

                    if(len>0)
                    {
                        byte[] tmp = new byte[len];
                        Array.Copy(data, 0, tmp, 0, len);

                        Packet packet = new Packet(tmp);

                        switch(packet.CommandInfo())
                        {
                            case Command.HndChek:
                                {
                                    string name = packet.DataToString();
                                    Station station = SessionManager.GetStations.Find(p => p.Name == name);

                                    if(station != null)
                                    {
                                        station.SocketInfo = s;
                                        return;
                                        ///////
                                    }

                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }
                    }

                    s.Close();
                }
                catch
                {
                }

            }
            catch
            {

            }
        }


        private static bool IsRunning
        {
            get
            {
                return is_running;
            }
        }

    }
}
