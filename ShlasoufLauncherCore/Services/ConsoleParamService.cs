using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public class ConsoleParamService : IConsoleParamService
    {
        public bool IsInstallMode()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1] == "install")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
