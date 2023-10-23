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
        public MegaFileDownloader(IMD5ComputeService md5Service, IStatusBarUpdateService statusUpdateService, String baseDlPath) : base(md5Service, statusUpdateService, baseDlPath) {}

        public override async Task<string> DownloadFile(string link, string md5)
        {
            var client = new MegaApiClient();

            try
            {
                await client.LoginAnonymousAsync();
                Uri fileLink = new Uri(link);

                var fileName = await GetFileName(link, client);
                var path = _baseDlPath + "\\" + fileName;
                bool fileValidity = await base.IsFileValid(path, md5);

                if (!fileValidity)
                {
                    File.Delete(path);
                    IProgress<double> progressHandler = new Progress<double>((percent) => _statusUpdateService.UpdateDownloadStatus(percent));
                    await client.DownloadFileAsync(fileLink, path, progressHandler);
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
            } finally
            {
                await client.LogoutAsync();
            }
        }

        public override async Task<string> GetFileName(string link)
        {
            var client = new MegaApiClient();
            await client.LoginAnonymousAsync();
            var fileName = await GetFileName(link, client);
            await client.LogoutAsync();
            return fileName;
        }

        private async Task<string> GetFileName(string link, MegaApiClient client)
        {
            Uri fileLink = new Uri(link);
            INodeInfo node = await client.GetNodeFromLinkAsync(fileLink);
            return node.Name;
        }
    }
}
