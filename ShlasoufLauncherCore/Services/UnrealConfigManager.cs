using ShlasoufLauncherCore.Models.Exceptions;
using ShlasoufLauncherCore.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Models.Services
{
    public class UnrealConfigManager : IUnrealConfigManager
    {
        private ILauncherSettingsService _settingsService;
        private IScreenResolutionService _screenServ;
        public UnrealConfigManager(ILauncherSettingsService settingsService, IScreenResolutionService screenServ)
        {
            _settingsService = settingsService;
            _screenServ = screenServ;
        }

        public async Task<List<UnrealConfigProperty>> getPropertiesFromConfigAsync(List<UnrealConfigProperty> properties, string configFilePath)
        {
            return await Task.Run(() => getPropertiesFromConfig(properties, configFilePath));

        }

        private List<UnrealConfigProperty> getPropertiesFromConfig(List<UnrealConfigProperty> properties, string configFilePath)
        {
            var path = _settingsService.GetInstallPath() + "\\" + configFilePath;
            var sortie = new List<UnrealConfigProperty>();
            var currentSection = "";
            foreach (var line in File.ReadLines(path))
            {
                if (properties.Count == 0)
                    break;

                // ...process line.
                var currentLine = line;
                if (currentLine.Contains("["))
                {
                    currentSection = currentLine.Substring(1, currentLine.Length - 2);
                }
                else
                {
                    for (int i = 0; i < properties.Count; i++)
                    {
                        var property = properties[i];
                        if (currentLine.Contains(property.Key) && currentSection.Equals(property.Section))
                        {
                            var current = property;
                            current.Value = currentLine.Substring(currentLine.LastIndexOf("=") + 1).Trim();
                            sortie.Add(current);
                            properties.Remove(property);
                            break;
                        }
                    }
                }
            }
            return sortie;
        }

        public async Task setPropertiesForConfigAsync(List<UnrealConfigProperty> properties, string configFilePath)
        {
            await Task.Run(() => setPropertiesForConfig(properties, configFilePath));
        }

        private async Task setPropertiesForConfig(List<UnrealConfigProperty> properties, string configFilePath)
        {
            var path = _settingsService.GetInstallPath() + "\\" + configFilePath;
            var sortie = new List<string>();
            var currentSection = "";
            foreach (var line in File.ReadLines(path))
            {
                var currentLine = line;
                if (currentLine.Contains("["))
                {
                    currentSection = currentLine.Substring(1, currentLine.Length - 2);
                }
                else
                {
                    for (int i = 0; i < properties.Count; i++)
                    {
                        var property = properties[i];
                        if (currentLine.Contains(property.Key) && currentSection.Equals(property.Section))
                        {
                            currentLine = property.ToString();
                            properties.Remove(property);
                            break;
                        }
                    }
                }
                sortie.Add(currentLine);
            }
            await File.WriteAllLinesAsync(path, sortie);
        }

        public async Task SetOptions(UnrealShlasoufOptions options)
        {
            
            var configPropertyList = new List<UnrealConfigProperty>() {
                new UnrealConfigProperty("WinDrv.WindowsClient","FullscreenViewportX", options.Resolution.X.ToString()),
                new UnrealConfigProperty("WinDrv.WindowsClient","FullscreenViewportY", options.Resolution.Y.ToString()),
                new UnrealConfigProperty("WinDrv.WindowsClient","MenuViewportX", options.Resolution.X.ToString()),
                new UnrealConfigProperty("WinDrv.WindowsClient","MenuViewportY", options.Resolution.Y.ToString()),
                new UnrealConfigProperty("IpDrv.TcpNetDriver","KeepAliveTime", options.KeepAliveTime)
            };

            var userPropertyList = new List<UnrealConfigProperty>() {
                new UnrealConfigProperty("Engine.PlayerInput","MouseSensitivity", options.MouseSensitivity),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","Desired43FOV", options.FOV),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","DesiredRatioX",options.Resolution.AspectRatioX.ToString()),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","DesiredRatioY", options.Resolution.AspectRatioY.ToString()),
                new UnrealConfigProperty("DefaultPlayer","Name", options.PlayerName)
            };

            try
            {
                await setPropertiesForConfig(configPropertyList, Properties.Resources.utConfigPath);
                await setPropertiesForConfig(userPropertyList, Properties.Resources.utUserPath);
            }catch(Exception e)
            {
                throw new CannotWriteOptionsException();
            }

        }

        private async Task SetOptionsWithMusic(UnrealShlasoufOptions options, double musicVolume)
        {

            var configPropertyList = new List<UnrealConfigProperty>() {
                new UnrealConfigProperty("WinDrv.WindowsClient","FullscreenViewportX", options.Resolution.X.ToString()),
                new UnrealConfigProperty("WinDrv.WindowsClient","FullscreenViewportY", options.Resolution.Y.ToString()),
                new UnrealConfigProperty("WinDrv.WindowsClient","MenuViewportX", options.Resolution.X.ToString()),
                new UnrealConfigProperty("WinDrv.WindowsClient","MenuViewportY", options.Resolution.Y.ToString()),
                new UnrealConfigProperty("ALAudio.ALAudioSubsystem","MusicVolume", musicVolume.ToString("0.00000",System.Globalization.CultureInfo.InvariantCulture)),
                new UnrealConfigProperty("IpDrv.TcpNetDriver","KeepAliveTime", options.KeepAliveTime)
            };

            var userPropertyList = new List<UnrealConfigProperty>() {
                new UnrealConfigProperty("Engine.PlayerInput","MouseSensitivity", options.MouseSensitivity),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","Desired43FOV", options.FOV),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","DesiredRatioX",options.Resolution.AspectRatioX.ToString()),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","DesiredRatioY", options.Resolution.AspectRatioY.ToString()),
                new UnrealConfigProperty("DefaultPlayer","Name", options.PlayerName)
            };

            try
            {
                await setPropertiesForConfig(configPropertyList, Properties.Resources.utConfigPath);
                await setPropertiesForConfig(userPropertyList, Properties.Resources.utUserPath);
            }
            catch (Exception e)
            {
                throw new CannotWriteOptionsException();
            }

        }

        public async Task SetDefaultOptions()
        {
            try
            {
                var defaultFOV = "90";
                var resolution = _screenServ.GetDeviceResolution();
                var defaultName = "Player";
                var sensi = "1.000000";
                var keepAlive = "0.004";
                var defaultOptions = new UnrealShlasoufOptions(resolution, defaultFOV, defaultName, sensi, keepAlive);
                await SetOptionsWithMusic(defaultOptions,0.12) ;
            }catch(Exception e)
            {
                throw new CannotWriteOptionsException();
            }
        }

        public async Task<UnrealShlasoufOptions> GetOptions()
        {
            var configPropertyList = new List<UnrealConfigProperty>() {
                new UnrealConfigProperty("WinDrv.WindowsClient","FullscreenViewportX", ""),
                new UnrealConfigProperty("WinDrv.WindowsClient","FullscreenViewportY", ""),
                new UnrealConfigProperty("WinDrv.WindowsClient","MenuViewportX", ""),
                new UnrealConfigProperty("WinDrv.WindowsClient","MenuViewportY", ""),
                new UnrealConfigProperty("IpDrv.TcpNetDriver","KeepAliveTime", "")
            };

            var userPropertyList = new List<UnrealConfigProperty>() {
                new UnrealConfigProperty("Engine.PlayerInput","MouseSensitivity", ""),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","Desired43FOV", ""),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","DesiredRatioX", ""),
                new UnrealConfigProperty("foxWSFix.foxPlayerInput","DesiredRatioY", ""),
                new UnrealConfigProperty("DefaultPlayer","Name", "")
            };

            try
            {
                var config = await getPropertiesFromConfigAsync(configPropertyList, Properties.Resources.utConfigPath);
                var user = await getPropertiesFromConfigAsync(userPropertyList, Properties.Resources.utUserPath);

                int x = Int32.Parse(config.First((e) => e.Key.Equals("FullscreenViewportX")).Value);
                int y = Int32.Parse(config.First((e) => e.Key.Equals("FullscreenViewportY")).Value);
                string keepAlive = config.First((e) => e.Key.Equals("KeepAliveTime")).Value;

                string name = user.First((e) => e.Key.Equals("Name")).Value;
                string fov = user.First((e) => e.Key.Equals("Desired43FOV")).Value;
                string sensi = user.First((e) => e.Key.Equals("MouseSensitivity")).Value;
                var res = _screenServ.FindInAvaillableResolutions(x, y);

                UnrealShlasoufOptions sortie = new UnrealShlasoufOptions(res, fov, name, sensi, keepAlive);
                return sortie;
            }catch(Exception e)
            {
                throw new UnableToFetchOptionsException();
            }
        }
    }
}
