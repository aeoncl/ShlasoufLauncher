using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShlasoufLauncherCore.Models.Exceptions;
using ShlasoufLauncherCore.Models.Services;
using ShlasoufLauncherCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.ViewModels
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly ILauncherSettingsService _launcherSettingsService;
        private readonly IFileDialogHelperService _fileDialogHelper;
        private readonly IShlasoufAPI _shlasoufAPI;
        private readonly IGameDownloaderService _gameDlService;
        private readonly IStatusBarUpdateService _statusUpdateService;
        private readonly IUnzipService _unzipService;
        private readonly IRegistryWriter _registryWriter;
        private readonly IUnrealConfigManager _utconfigManager;
        private readonly MainViewModel _mainVM;

        public SetupViewModel(ILauncherSettingsService launcherSettingsService, 
            IFileDialogHelperService fileDialogHelper, MainViewModel mainVM, 
            IShlasoufAPI shlasoufAPI, IGameDownloaderService gameDlService, 
            IStatusBarUpdateService statusUpdate, IUnzipService unzipService,
            IRegistryWriter registryWriterService,IUnrealConfigManager configManager)
        {
            _launcherSettingsService = launcherSettingsService;
            _fileDialogHelper = fileDialogHelper;
            _mainVM = mainVM;
            _shlasoufAPI = shlasoufAPI;
            _gameDlService = gameDlService;
            _statusUpdateService = statusUpdate;
            _unzipService = unzipService;
            _registryWriter = registryWriterService;
            _utconfigManager = configManager;
            BrowseButtonCommand = new RelayCommand(BrowseButtonClickHandler);
            SuivantButtonCommand = new RelayCommand(SuivantButtonClickHandler);
            
            SelectedPath = Environment.CurrentDirectory;
        }

        

        public RelayCommand BrowseButtonCommand { get; }

        public RelayCommand SuivantButtonCommand { get; }



        public string SelectedPath { get; set; }

        private void BrowseButtonClickHandler()
        {
           SelectedPath = _fileDialogHelper.OpenFileBrowser(SelectedPath);
           RaisePropertyChanged("SelectedPath");
        }

        private async Task runDirectXSetup()
        {
            _statusUpdateService.UpdateStatus("Installation de DirectX 9.0c");
            _statusUpdateService.BlinkProgressBar(true);
            string name = Environment.CurrentDirectory + "\\" + "redist" + "\\DXSETUP.exe";
            Process process = new Process();
            process.StartInfo.FileName = name;
            process.StartInfo.Arguments = "/silent";
            process.StartInfo.UseShellExecute = false;
            await Task.Run(() => {
                   try
                   {
                       process.Start();
                       process.WaitForExit();
                   }catch(Win32Exception e)
                   {
                       Console.WriteLine("Erreur Installation de DirectX :" + e.StackTrace);
                   }
               });
        }
        private async void SuivantButtonClickHandler()
        {

            _mainVM.ShowPlaySetup = true;
            _statusUpdateService.UpdateStatus("Récupération des liens de téléchargement");
            var packages = await _shlasoufAPI.GetInstall();
            if(packages != null)
            {
               string fileDownloaded = await _gameDlService.DownloadGame(packages.utgame, packages.utgamehash);
               if(fileDownloaded != "")
                {
                    try
                    {
                        await _unzipService.Unzip(fileDownloaded, SelectedPath);
                        _launcherSettingsService.writeGamePath(SelectedPath + "\\" + Properties.Resources.rootFolderName);
                        _registryWriter.WriteGameKey();
                        _gameDlService.DeleteTempFiles(fileDownloaded);
                        await _utconfigManager.SetDefaultOptions();
                        await runDirectXSetup();
                        _mainVM.ShowPlayButton = true;
                        _mainVM.ShowPlay = true;
                    }
                    catch (UnableToFetchOptionsException e)
                    {
                        _mainVM.ShowPlayButton = true;
                        _mainVM.ShowPlay = true;
                    }
                    catch (CannotWriteIntoRegistryException e)
                    {
                        _statusUpdateService.UpdateStatus("Impossible d'écrire dans le registre ! êtes vous admin?");
                    }
                    catch (Exception e)
                    {
                        _statusUpdateService.UpdateStatus("Une erreur est survenue! Veuillez réessayer... :(");
                    }
                }
            }
            else
            {
                _statusUpdateService.UpdateStatus("Le serveur Shlasouf ne réponds pas, rééssayez plus tard.");
            }
        }
    }
}
