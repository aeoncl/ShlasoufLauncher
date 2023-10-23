using ShlasoufLauncherCore.Models;
using ShlasoufLauncherCore.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShlasoufLauncherCore.Services
{
    public class LauncherSettingsService : ILauncherSettingsService
    {
         private string applicationPath, launcherConfigFile, configFilePath;

    public LauncherSettingsService()
    {
        applicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        launcherConfigFile = Properties.Resources.launcherConfigFile;
        configFilePath = applicationPath + "\\" + Properties.Resources.appDataName +"\\" + launcherConfigFile;
        tryCreateLauncherSettingsFile();
    }

    private void tryCreateLauncherSettingsFile()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
            if (!File.Exists(configFilePath))
            {
                var configFile = new LauncherSettings();
                writeConfigFile(configFile);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }

    public LauncherSettings getConfig()
    {
        try
        {
            if (File.Exists(configFilePath))
            {
                using (StreamReader reader = new StreamReader(configFilePath))
                {
                    string json = reader.ReadToEnd();
                    LauncherSettings conf = JsonSerializer.Deserialize<LauncherSettings>(json);
                    return conf;
                }
            }
            else
            {
                tryCreateLauncherSettingsFile();
                return getConfig();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            throw new CannotReadConfigFileException();
        }
    }

    public void writeGamePath(string gamePath)
    {
        var config = this.getConfig();
        config.gamePath = gamePath;
        this.writeConfigFile(config);
    }

    public void writeConfigFile(LauncherSettings conf)
    {
        try
        {
            string configJson = JsonSerializer.Serialize(conf);
            using (StreamWriter writer = new StreamWriter(configFilePath, false))
            {
                writer.Write(configJson);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

        public string GetGameKey()
        {
            var config = this.getConfig();
            return config.cdKey;
        }

        public string GetInstallPath()
        {
            var config = this.getConfig();
            return config.gamePath;
        }
        public void removePath()
        {
            var config = this.getConfig();
            config.gamePath = "";
            writeConfigFile(config);
        }

        public string GetServerURL()
        {
            var config = this.getConfig();
            return config.server;
        }
    }

}
