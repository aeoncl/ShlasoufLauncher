using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models.Exceptions
{
    public class CannotReadConfigFileException : Exception
    {
        public CannotReadConfigFileException(string message = "Cannot read config file") : base(message)
        {
        }
    }
}
