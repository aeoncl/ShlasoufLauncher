using Microsoft.Win32;
using ShlasoufLauncherCore.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public class RegistryWriter : IRegistryWriter
    {
        private ILauncherSettingsService _settingsService;
        public RegistryWriter(ILauncherSettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public void WriteGameKey()
        {
            try
            {
                string gameKey = _settingsService.GetGameKey();
                using (var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                RegistryView.Registry32))
                {
                    string regPath = Properties.Resources.regPath;

                    using (var clsid32 = view32.CreateSubKey(@"SOFTWARE\" + regPath, true))
                    {
                        clsid32.SetValue(Properties.Resources.regKey, gameKey);
                    }
                }
            }
            catch(System.UnauthorizedAccessException e)
            {
                throw new CannotWriteIntoRegistryException(e.Message);
            }
        }
    }
}
