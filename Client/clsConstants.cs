using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal static class Constants
    {
        private static object mlocker = new object();
        private static ushort port = 967;

        private static string configPath = Environment.GetEnvironmentVariable("windir") + "\\ycc_edu.bin";
        private static string sysPath = Environment.GetEnvironmentVariable("windir") + "\\ycc_edu.exe";

        internal static ushort Port
        {
            get
            {
                return port;
            }

        }
        internal static string ConfigPath
        {
            get
            {
                return configPath;
            }

        }

        internal static string SystemPath
        {
            get
            {
                return sysPath;
            }

        }

        internal static object MyLocker
        {
            get
            {
                return mlocker;
            }
        }
    }
}
