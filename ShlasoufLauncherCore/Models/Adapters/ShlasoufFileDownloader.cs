﻿using ShlasoufLauncherCore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Models.Adapters
{
    public class ShlasoufFileDownloader : FileDownloader, IFileDownloader
    {
        public ShlasoufFileDownloader(IMD5ComputeService md5Service, IStatusBarUpdateService statusUpdateService, String baseDlPath) : base(md5Service, statusUpdateService, baseDlPath) { }

        public override async Task<string> DownloadFile(string link, string md5)
        {
            try
            {
                var fileName = await GetFileName(link);
                string path = _baseDlPath + "\\" + fileName;
                bool fileValidity = await base.IsFileValid(path, md5);
                if (!fileValidity) { 
                    File.Delete(path);
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                        Uri fileLink = new Uri(link);
                        await webClient.DownloadFileTaskAsync(fileLink, path);
                    }
                }

                fileValidity = await base.IsFileValid(path, md5);
                if (!fileValidity)
                {
                    _statusUpdateService.UpdateStatus("Le fichier téléchargé est corrompu!");
                    throw new Exception();
                }
                return path;
            }
            catch (Exception e)
            {
                throw new Exception(e.StackTrace);
            }
        }

        public override Task<string> GetFileName(string link)
        {
            var indexOf = link.LastIndexOf("/");
            string fileName = link.Substring(indexOf + 1);
            return Task.FromResult(fileName);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _statusUpdateService.UpdateDownloadStatus(e.ProgressPercentage);
        }
    }
}
