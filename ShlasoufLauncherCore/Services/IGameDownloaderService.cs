using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public interface IGameDownloaderService
    {
        public Task<string> DownloadGame(string[] links, string md5);
        void DeleteTempFiles(string path);
    }
}
