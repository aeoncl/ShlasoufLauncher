using ShlasoufLauncherCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public interface ILauncherSettingsService
    {
        public LauncherSettings getConfig();
        public void writeConfigFile(LauncherSettings conf);
        public void writeGamePath(string gamePath);
        public string GetGameKey();
        public string GetInstallPath();

        public string GetServerURL();

        public void removePath();
    }
}
