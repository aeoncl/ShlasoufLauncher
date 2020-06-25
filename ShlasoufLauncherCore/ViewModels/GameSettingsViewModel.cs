using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.DependencyInjection;
using ShlasoufLauncherCore.Models;
using ShlasoufLauncherCore.Models.Exceptions;
using ShlasoufLauncherCore.Models.Services;
using ShlasoufLauncherCore.Services;
using ShlasoufLauncherCore.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ShlasoufLauncherCore.ViewModels
{
   public class GameSettingsViewModel : ViewModelBase
    {
        private IUnrealConfigManager _configManager;
        private IScreenResolutionService _screenResService;
        private ScreenResolution _selectedRes;
        public GameSettingsViewModel(IUnrealConfigManager configManager, IScreenResolutionService screenRes)
        {
            SaveButtonEnabled = true;
            SaveConfigBtnPressed = new RelayCommand(SaveConfigBtnPressedHandler);
            DefaultConfigBtnPressed = new RelayCommand(DefaultConfigBtnPressedHandler);
            _screenResService = screenRes;
            _configManager = configManager;
            GetProperties();
        }

        private void SaveConfigBtnPressedHandler()
        {
            var test = new Sound(true);
            test.Play(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Assets\\" + "nicodomination.mp3");
            UnrealShlasoufOptions options = new UnrealShlasoufOptions(SelectedRes, FOV, PlayerName, MouseSensi, KeepAliveTime);
            _configManager.SetOptions(options);
        }

        private void DefaultConfigBtnPressedHandler()
        {
            _configManager.SetDefaultOptions();
        }

        public List<ScreenResolution> AvaillableRes => _screenResService.AvaillableResolutions;
        public RelayCommand SaveConfigBtnPressed { get; set; }
        public RelayCommand DefaultConfigBtnPressed { get; set; }

        public string MouseSensi { get; set; }
        public ScreenResolution SelectedRes 
        { 
            get {
                return _selectedRes;
            }
            set {
                _selectedRes = value;
                RaisePropertyChanged("SelectedRes");
            }
        }

        public string FOV { get; set; }

        public string PlayerName { get; set; }

        public bool SaveButtonEnabled { get; set; }

        public string KeepAliveTime { get; set; }

        private async void GetProperties()
        {
            try
            {
                var options = await _configManager.GetOptions();
                SelectedRes = options.Resolution;
                PlayerName = options.PlayerName;
                FOV = options.FOV;
                MouseSensi = options.MouseSensitivity;
                KeepAliveTime = options.KeepAliveTime;
                RaisePropertyChanged("SelectedRes");
                RaisePropertyChanged("PlayerName");
                RaisePropertyChanged("MouseSensi");
                RaisePropertyChanged("KeepAliveTime");
                RaisePropertyChanged("FOV");
            }catch(UnableToFetchOptionsException e)
            {
                PlayerName = "Erreur";
                RaisePropertyChanged("PlayerName");
                SaveButtonEnabled = false;
                RaisePropertyChanged("SaveButtonEnabled");

            }

        }

    }
}
