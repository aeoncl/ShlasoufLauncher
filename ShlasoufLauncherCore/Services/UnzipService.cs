using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShlasoufLauncherCore.Services
{
    public class UnzipService : IUnzipService
    {
        private IStatusBarUpdateService _statusService;
        public UnzipService(IStatusBarUpdateService statusUpdateService)
        {
            _statusService = statusUpdateService;
        }
        public async Task Unzip(string file, string targetDir)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(file))
                {
                    zip.ExtractProgress += Zip_ExtractProgress;
                    await Task.Run(() => zip.ExtractAll(targetDir, ExtractExistingFileAction.OverwriteSilently));
                }
            }
            catch(Exception e)
            {
                throw new Exception();
            }
        }

        private void Zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {
                double percent = Math.Round((double)(100 * e.EntriesExtracted) / e.EntriesTotal);
                _statusService.UpdateProgressBar(percent);
                _statusService.UpdateStatus("Extraction: " + e.CurrentEntry.ToString().Substring(10) + " - " + percent + " % ");
            }
        }
    }
}
