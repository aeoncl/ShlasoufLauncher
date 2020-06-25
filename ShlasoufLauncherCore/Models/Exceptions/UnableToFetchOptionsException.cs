using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models.Exceptions
{
    public class UnableToFetchOptionsException : Exception
    {
        public UnableToFetchOptionsException(string message = "G PAS REUSSI A CHOPPER LES OPTIONS WESH MEKK") : base(message)
        {
        }
    }
}
