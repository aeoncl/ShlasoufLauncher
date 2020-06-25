using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models
{
    public class UnrealShlasoufOptions
    {
        public UnrealShlasoufOptions()
        {

        }
        public UnrealShlasoufOptions(ScreenResolution res, string FOV, string PlayerName, string sensi, string keepAlive)
        {
            this.PlayerName = PlayerName;
            Resolution = res;
            this.FOV = FOV;
            this.MouseSensitivity = sensi;
            this.KeepAliveTime = keepAlive;
        }
        public string PlayerName { get; set; }
        public ScreenResolution Resolution { get; set; }
        public string FOV { get; set; }

        public string MouseSensitivity { get; set; }

        public string KeepAliveTime { get; set; }

    }
}
