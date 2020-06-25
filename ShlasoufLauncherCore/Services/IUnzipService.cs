using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public interface IUnzipService
    {
        public Task Unzip(string file, string targetDir);
    }
}
