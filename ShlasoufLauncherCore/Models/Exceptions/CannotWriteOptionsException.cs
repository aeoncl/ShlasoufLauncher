using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models.Exceptions
{
    public class CannotWriteOptionsException : Exception
    {
        public CannotWriteOptionsException(string message = "JE SAIS PAS ECRIRE LES OPTIONS PUTAIN") : base(message)
        {
        }
    }
}
