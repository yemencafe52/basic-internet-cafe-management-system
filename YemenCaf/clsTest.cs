//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Threading;
//using System.Net;

//namespace YemenCafe
//{
//    static class clsEntryPoint
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Server.Start();
//            IPAddress ip = IPAddress.Parse("127.0.0.1");
//            SocketCommander s = new SocketCommander(ip);
//            s.Send(new Packet(Command.PINGTO));
//            Server.Stop();

//            s.Send(new Packet(Command.PINGTO));

//            //bool res = ParamInfo.GetParamInfo();

//            //res = false;


//            //res = ParamInfo.UpdateParamInfo(120, 6, "C:\\", 5);

//            //// Utilities.TestDb();

//            //User user = new User(UserManager.GenerateNewUserNumber(), "user", "123", 1);
//            //UserManager.AddNewUser(user);
//            //UserManager.UpdateUserInfo(user);

//            //UserManager.Login("admin","admin",ref user);

//            //SessionManager.Init();// test
//            //SessionManager.OpenNewSession(SessionManager.GetStations[0]);
//            //SessionManager.OpenNewSession(SessionManager.GetStations[2]);

//            //int x = 0;
//            //while (x <= 3)
//            //{
//            //    Console.Clear();
//            //    Console.WriteLine("NAME\tSTATE\tTIME\tCOST");


//            //    for (int i = 0; i < ParamInfo.StationsCount; i++)
//            //    {
//            //        Console.WriteLine(SessionManager.GetStations[i].Name + "\t" + SessionManager.GetStations[i].SessionInfo.SessionState.ToString() + "\t" + SessionManager.GetStations[i].SessionInfo.TimeLeft.ToString() + "\t" + SessionManager.GetStations[i].SessionInfo.Cost.ToString());
//            //    }

//            //    x++;
//            //    Thread.Sleep(1000);

//            //}


//            //SessionManager.CloseSession(SessionManager.GetStations[0]);
//            //x = 0;
//            //while (x <= 3)
//            //{
//            //    Console.Clear();
//            //    Console.WriteLine("NAME\tSTATE\tTIME\tCOST");


//            //    for (int i = 0; i < ParamInfo.StationsCount; i++)
//            //    {
//            //        Console.WriteLine(SessionManager.GetStations[i].Name + "\t" + SessionManager.GetStations[i].SessionInfo.SessionState.ToString() + "\t" + SessionManager.GetStations[i].SessionInfo.TimeLeft.ToString() + "\t" + SessionManager.GetStations[i].SessionInfo.Cost.ToString());
//            //    }

//            //    x++;
//            //    Thread.Sleep(1000);

//            //}


//            //SessionManager.Shutdown();



//            //Application.EnableVisualStyles();
//            //Application.SetCompatibleTextRenderingDefault(false);
//            //Application.Run(new frmMain());
//        }
//    }
//}
