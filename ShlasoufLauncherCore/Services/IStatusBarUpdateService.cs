using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public interface IStatusBarUpdateService
    {
        public void UpdateStatus(string status);
        public void BlinkProgressBar(bool status);
        public void UpdateDownloadStatus(double percent);
        public void UpdateProgressBar(double percent);
    }
}
