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
        protected readonly string baseDlPath;
        public FileDownloader(IMD5ComputeService md5Service, IStatusBarUpdateService statusUpdateService)
        {
            _md5Service = md5Service;
            _statusUpdateService = statusUpdateService;

            var applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            baseDlPath = applicationPath + "\\" + Properties.Resources.appDataName ;
        }

        public abstract Task<string> DownloadFile(string link, string md5);

        public async Task<bool> IsFileValid(string path, string md5)
        {
            bool status = false;
            if (File.Exists(path))
            {
                string fileMD5 = await _md5Service.ComputeMD5Async(path);
                if (fileMD5 == md5)
                {
                    status = true;
                }
            }
            return status;
        }
    }
}
