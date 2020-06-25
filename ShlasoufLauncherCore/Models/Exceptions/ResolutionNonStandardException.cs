using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models.Exceptions
{
    public class ResolutionNonStandardException : Exception
    {
        public ResolutionNonStandardException(string message = "Your resolution is shit, sorry not sorry.") : base(message)
        {
        }
    }
}
