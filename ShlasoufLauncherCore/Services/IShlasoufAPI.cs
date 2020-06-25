using ShlasoufLauncherCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public interface IShlasoufAPI
    {
        public Task<UTPackages> GetInstall();
    }
}
