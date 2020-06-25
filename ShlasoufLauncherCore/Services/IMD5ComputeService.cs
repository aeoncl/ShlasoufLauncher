using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public interface IMD5ComputeService
    {
        public string ComputeMD5(string filePath);
        public Task<string> ComputeMD5Async(string filePath);
    }
}
