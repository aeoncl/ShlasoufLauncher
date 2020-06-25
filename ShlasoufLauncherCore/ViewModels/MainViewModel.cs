using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.DependencyInjection;
using ShlasoufLauncherCore.Models;
using ShlasoufLauncherCore.Models.Services;
using ShlasoufLauncherCore.Services;
using ShlasoufLauncherCore.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;

namespace ShlasoufLauncherCore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _showPlay, _showSetup, _showPlaySetup, _showPlayBtn, _showInstallBtn, _settingsOpenned;
        private ILauncherSettingsService _launcherSettings;
        private IFileDialogHelperService _fileDialogHelper;
        private IServiceProvider _serviceProv;
        private Sound _player;
        public MainViewModel(IConsoleParamService consoleParamService, ILauncherSettingsService launcherSettingsService, IFileDialogHelperService fds, IServiceProvider serviceProv)
        {
            IsMainWindowEnabled = true;
            _fileDialogHelper = fds;
            PlayClick = new RelayCommand(PlayClickHandler);
            InstallClick = new RelayCommand(InstallClickHandler);
            ClickMusicBox = new RelayCommand(ClickMusicBoxHandler);
            SettingsBtnCommand = new RelayCommand(SettingsBtnHandler);
            BrowseForTheGameCommand = new RelayCommand(ClickBrowseGameHandler);
            _serviceProv = serviceProv;
            _player = new Sound();
            _launcherSettings = launcherSettingsService;
            if (consoleParamService.IsInstallMode())
            {
                ShowSetup = true;
            }
            else
            {
                ShowPlay = true;
            }

            if(launcherSettingsService.GetInstallPath() != "")
            {
                ShowPlayButton = true;
            }
            else
            {
                ShowInstallButton = true;
            }
        }

        public bool IsMainWindowEnabled { get; set; }

        private void SettingsBtnHandler()
        {
            if (!_settingsOpenned)
            {
                var window = _serviceProv.GetRequiredService<GameSettingsWindow>();
                window.Show();
                window.Closed += ((e, o) => {
                    IsMainWindowEnabled = true;
                    RaisePropertyChanged("IsMainWindowEnabled");
                    _settingsOpenned = false;
                });
                _settingsOpenned = true;
                IsMainWindowEnabled = false;
                RaisePropertyChanged("IsMainWindowEnabled");


            }
        }

        private void ClickBrowseGameHandler()
        {
           string path = _fileDialogHelper.OpenFileBrowser(Environment.CurrentDirectory);
            if(File.Exists(path+"\\" + Properties.Resources.exePath))
            {
                _launcherSettings.writeGamePath(path);
                this.ShowPlayButton = true;
            }
        }
        public RelayCommand SettingsBtnCommand { get; }

        public RelayCommand ClickMusicBox { get; }
        public bool MusicBoxStatus => _player.IsPlaying;
        private void ClickMusicBoxHandler()
        {
            if (_player.IsPlaying)
            {
                _player.Pause();
            }
            else
            {
                _player.Play();
            }
            RaisePropertyChanged("MusicBoxStatus");
        }

        private void PlayClickHandler()
        {
            string utPath = _launcherSettings.GetInstallPath() + @"\" + Properties.Resources.exePath;
            if (File.Exists(utPath))
            {
                Process.Start(utPath);
                App.Current.Shutdown();
            }
            else
            {
                _launcherSettings.removePath();
                this.ShowInstallButton = true;
            }

        }

        private void InstallClickHandler()
        {
            string name = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            Process process = new Process();
            process.StartInfo.FileName = name;
            process.StartInfo.Arguments = "install";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Verb = "runas";
            try
            {
                process.Start();
                App.Current.Shutdown();
            }catch(Win32Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }

        public RelayCommand BrowseForTheGameCommand { get; set; }

        public RelayCommand PlayClick { get; set; }
        public RelayCommand InstallClick { get; set; }

        public bool ShowPlay {
            get => _showPlay;
            set
            {
                if (value)
                {
                    this._player.Pause();
                }
                _showPlay = value;
                _showSetup = !value;
                _showPlaySetup = !value;
                RaisePropertyChanged("ShowPlay");
                RaisePropertyChanged("ShowSetup");
                RaisePropertyChanged("ShowPlaySetup");
            }
        }
        public bool ShowSetup
        {
            get => _showSetup;
            set
            {
                if (value)
                {
                    _player.SetVolume(10);
                    _player.MediaPlayer.MediaEnded += (o, i) => RaisePropertyChanged("MusicBoxStatus");
                    _player.Play(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Assets\\" + "music0.mp3");
                }
                _showSetup = value;
                _showPlay = !value;
                _showPlaySetup = !value;
                RaisePropertyChanged("ShowPlay");
                RaisePropertyChanged("ShowSetup");
                RaisePropertyChanged("ShowPlaySetup");
            }
        }
        public bool ShowPlaySetup
        {
            get => _showPlaySetup;
            set
            {
                _showPlaySetup = value;
                _showSetup = !value;
                _showPlay = !value;
                RaisePropertyChanged("ShowPlay");
                RaisePropertyChanged("ShowSetup");
                RaisePropertyChanged("ShowPlaySetup");
            }
        }

        public bool ShowPlayButton
        {
            get => _showPlayBtn;
            set
            {
                _showPlayBtn = value;
                _showInstallBtn = !value;
                ShowUpdateButton = !value;
                RaisePropertyChanged("ShowPlayButton");
                RaisePropertyChanged("ShowInstallButton");
                RaisePropertyChanged("ShowUpdateButton");
            }
        }
        public bool ShowInstallButton
        {
            get => _showInstallBtn;
            set
            {
                _showInstallBtn = value;
                _showPlayBtn = !value;
                ShowUpdateButton = !value;
                RaisePropertyChanged("ShowPlayButton");
                RaisePropertyChanged("ShowInstallButton");
                RaisePropertyChanged("ShowUpdateButton");
            }
        }
        public bool ShowUpdateButton { get; set; }

    }
}
