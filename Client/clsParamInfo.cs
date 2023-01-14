using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    internal static class ParamInfo
    {
        private static string ip = "";
        private static string password = "";
        private static string name = "";
        internal static string IP
        {
            get
            {
                return ip;
            }

        }
        internal static string Password
        {
            get
            {
                return password;
            }

        }
        internal static string Name
        {
            get
            {
                return name;
            }

        }
        internal static bool ReadConfig()
        {
            bool res = false;

            try
            {
                FileStream fs = File.Open(Constants.ConfigPath, FileMode.Open);
                byte[] ar = new byte[fs.Length];
                fs.Read(ar, 0, ar.Length);
                fs.Close();

                string data = Encoding.UTF32.GetString(ar);
                string[] info = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                
                if(info.Count() == 3)
                {
                    ip = info[0];
                    password = info[1];
                    name = info[2];
                    res = true;
                }
              
            }

            catch
            {

            }

            return res;

        }
        internal static bool UpdateConfig(
               string ip ,
               string password ,
              string name 
        )
        {
            bool res = false;

            try
            {
                string data = ip + ";" + password + ";" + name + ";";
                byte[] ar = Encoding.UTF32.GetBytes(data);
                
                if (data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Count() == 3)
                {
                    FileStream fs = File.Open(Constants.ConfigPath, FileMode.OpenOrCreate);
                    fs.Write(ar, 0, ar.Length);
                    fs.Close();
                }

                res = true;

            }

            catch
            {

            }

            return res;

        }
    }
}
