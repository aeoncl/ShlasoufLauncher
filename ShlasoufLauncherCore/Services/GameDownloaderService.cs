﻿using ShlasoufLauncherCore.Models.Adapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public class GameDownloaderService : IGameDownloaderService
    {
        private List<IFileDownloader> _fileDownloaders;

        private string _baseDlPath;

        public GameDownloaderService(IMD5ComputeService md5Service, IStatusBarUpdateService statusService)
        {
            var applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this._baseDlPath = applicationPath + "\\" + Properties.Resources.appDataName;
            _fileDownloaders = new List<IFileDownloader>() { new MegaFileDownloader(md5Service, statusService, _baseDlPath), new ShlasoufFileDownloader(md5Service, statusService, _baseDlPath) };
        }
        public async Task<string> DownloadGame(string[] links, string md5)
        {
            string fileName = "";
            foreach (string link in links)
            {
                try
                {
                    if (link.Contains("mega"))
                    {
                        fileName = await _fileDownloaders[0].DownloadFile(link, md5);
                        break;
                    }

                    if (link.Contains("shlasouf"))
                    {
                        fileName = await _fileDownloaders[1].DownloadFile(link, md5);
                        break;
                    }
                }
                catch (Exception e)
                {
                    //provider failing, go to the next one.
                }
            }
            return fileName;
        }

        public void DeleteTempFiles(string path)
        {
            try
            {
                File.Delete(path);
            }catch(Exception e)
            {

            }
        }
    }
}
