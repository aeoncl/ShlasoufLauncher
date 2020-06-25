using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models.Exceptions
{
    public class CannotWriteIntoRegistryException : Exception
    {
        public CannotWriteIntoRegistryException(string message = "Cannot write the registry, you lack admin rights") : base(message)
        {
        }
    }
}
