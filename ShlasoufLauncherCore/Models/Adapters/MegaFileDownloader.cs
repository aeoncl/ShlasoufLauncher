using CG.Web.MegaApiClient;
using ShlasoufLauncherCore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Models.Adapters
{
    public class MegaFileDownloader : FileDownloader, IFileDownloader
    {
        public MegaFileDownloader(IMD5ComputeService md5Service, IStatusBarUpdateService statusUpdateService) : base(md5Service, statusUpdateService) { }

        public override async Task<string> DownloadFile(string link, string md5)
        {
            try
            {
                var client = new MegaApiClient();
                client.LoginAnonymous();
                Uri fileLink = new Uri(link);

                INodeInfo node = client.GetNodeFromLink(fileLink);
                var path = baseDlPath + "\\" + node.Name;
                bool fileValidity = await base.IsFileValid(path, md5);

                if (!fileValidity)
                {
                    File.Delete(path);
                    IProgress<double> progressHandler = new Progress<double>((percent) => _statusUpdateService.UpdateDownloadStatus(percent));
                    await client.DownloadFileAsync(fileLink, path, progressHandler);
                    client.Logout();
                    fileValidity = await base.IsFileValid(path, md5);
                    if (!fileValidity)
                    {
                        _statusUpdateService.UpdateStatus("Le fichier téléchargé est corrompu...");
                        throw new Exception();
                    }
                }
                return path;
            }
            catch (Exception e)
            {
                throw new Exception(e.StackTrace);
            }
        }
    }
}
