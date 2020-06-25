using ShlasoufLauncherCore.Models;
using ShlasoufLauncherCore.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public class MD5ComputeService : IMD5ComputeService
    {

        private readonly IStatusBarUpdateService _statusService;
        public MD5ComputeService(IStatusBarUpdateService statusService)
        {
            _statusService = statusService;
        }

        public string ComputeMD5(string filePath)
        {
            _statusService.UpdateStatus("Vérification du fichier téléchargé...");
            _statusService.BlinkProgressBar(true);
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        var hash = md5.ComputeHash(stream);
                        _statusService.BlinkProgressBar(false);
                        return BitConverter.ToString(hash).Replace("-", "");
                    }
                }
            }catch(Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
            }
            _statusService.BlinkProgressBar(false);
            return "";
        }

        public async Task<string> ComputeMD5Async(string filePath)
        {
            return await Task<string>.Run(() => ComputeMD5(filePath));
        }
    }
}
