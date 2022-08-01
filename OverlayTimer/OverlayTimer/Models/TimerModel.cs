using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OverlayTimer
{
    internal class TimerModel : INotifyPropertyChanged
    {
        private string _timer;

        public string Timer
        {
            get { return _timer; }
            set
            {
                _timer = value;
                OnPropertyChanged(nameof(Timer));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
