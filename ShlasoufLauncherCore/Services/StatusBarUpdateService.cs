using ShlasoufLauncherCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public class StatusBarUpdateService : IStatusBarUpdateService
    {
        private StatusBarViewModel _vm;
        public StatusBarUpdateService(StatusBarViewModel vm)
        {
            _vm = vm;
        }
        public void UpdateStatus(string status)
        {
            _vm.Status = status;
        }

        public void BlinkProgressBar(bool status)
        {
            _vm.ProgressBarBlink = status;
            _vm.ProgressPercentage = 0;
        }

        public void UpdateProgressBar(double percent)
        {
            _vm.ProgressPercentage = (int)percent;
        }

        public void UpdateDownloadStatus(double percent)
        {
            UpdateProgressBar(percent);
            UpdateStatus(String.Format("Téléchargement du jeu: {0:.##} %", percent));
        }
    }
}
