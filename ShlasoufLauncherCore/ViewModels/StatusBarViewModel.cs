using GalaSoft.MvvmLight;

namespace ShlasoufLauncherCore.ViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        private string _status = "";
        private int _percent = 0;
        private bool _progressBarBlink = false;
        public string Status {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        public int ProgressPercentage
        {
            get { return this._percent; }
            set
            {
                this._percent = value;
                RaisePropertyChanged("ProgressPercentage");
            }
        }

        public bool ProgressBarBlink
        {
            get => _progressBarBlink;
            set
            {
                _progressBarBlink = value;
                RaisePropertyChanged("ProgressBarBlink");
            }
        }
    }
}
