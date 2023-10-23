using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Models.Adapters
{
    public interface IFileDownloader
    {
        public Task<string> DownloadFile(string link, string md5);
        public Task<string> GetFileName(string link);
        public Task<bool> IsFileValid(string path, string md5);
    }
}
