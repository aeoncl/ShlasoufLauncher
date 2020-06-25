using ShlasoufLauncherCore.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public class ShlasoufAPI : IShlasoufAPI
    {
        public async Task<UTPackages> GetInstall()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiPath = "http://" + Properties.Resources.defaultServer + "/" + Properties.Resources.apiInstallPath;
                    var responseJson = await client.GetStringAsync(apiPath);
                    var response = JsonSerializer.Deserialize<UTPackages>(responseJson);
                    return response;
                }
            }catch(Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }
            return null;
        }
    }
}
