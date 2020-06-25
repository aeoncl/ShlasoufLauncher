using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models
{
    public class LauncherSettings
    {
        public LauncherSettings()
        {
            gamePath = "";
            server = Properties.Resources.defaultServer;
            cdKey = Properties.Resources.defaultCDKey;
        }
        public string gamePath { get; set; }
        public string server { get; set; }
        public string cdKey { get; set; }
    }
}
