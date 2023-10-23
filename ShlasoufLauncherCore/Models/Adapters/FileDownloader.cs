using ShlasoufLauncherCore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Models.Adapters
{
    public abstract class FileDownloader : IFileDownloader
    {

        protected readonly IMD5ComputeService _md5Service;
        protected readonly IStatusBarUpdateService _statusUpdateService;
        protected readonly string _baseDlPath;
        public FileDownloader(IMD5ComputeService md5Service, IStatusBarUpdateService statusUpdateService, string baseDlPath)
        {
            _md5Service = md5Service;
            _statusUpdateService = statusUpdateService;
            _baseDlPath = baseDlPath;
        }

        public abstract Task<string> DownloadFile(string link, string md5);

        public abstract Task<string> GetFileName(string link);

        public async Task<bool> IsFileValid(string path, string md5)
        {
            bool status = false;
            if (File.Exists(path))
            {
                string fileMD5 = await _md5Service.ComputeMD5Async(path);
                if (fileMD5.ToUpper().Equals(md5.ToUpper()))
                {
                    status = true;
                }
            }
            return status;
        }
    }
}
