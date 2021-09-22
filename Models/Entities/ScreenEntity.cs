using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Light.Models.Entities
{
    public sealed class ScreenEntity : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;


        #endregion

        #region Values

        private bool _active;
        private int _startTime = 1380;
        private int _endTime = 720;
        private float _currentColorTemperature = 6600;
        private float _currentBrightness = 1f;
        private int _dayColorTemperature = 6600;
        private float _dayBrightness = 1f;
        private int _nightColorTemperature = 4000;
        private float _nightBrightness = 1f;
        private float _uiOpacity = 1f;

        #endregion

        #region Constructors

        public Screen Instance { get; set; }
        public string SysName { get; set; }
        public string Name { get; set; }

        public float Opacity
        {
            get => _uiOpacity;
            private set
            {
                _uiOpacity = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => _active;
            set
            {
                _active = value;
                Opacity = value ? 1f : 0.5f;
                OnPropertyChanged();
            }
        }

        public int StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        public int EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public float CurrentColorTemperature
        {
            get => _currentColorTemperature;
            set
            {
                _currentColorTemperature = value;
                OnPropertyChanged();
            }
        }

        public float CurrentBrightness
        {
            get => _currentBrightness;
            set
            {
                _currentBrightness = value;
                OnPropertyChanged();
            }
        }

        public int DayColorTemperature
        {
            get => _dayColorTemperature;
            set
            {
                _dayColorTemperature = value;
                OnPropertyChanged();
            }
        }
        public float DayBrightness
        {
            get => _dayBrightness;
            set
            {
                _dayBrightness = value;
                OnPropertyChanged();
            }
        }

        public int NightColorTemperature
        {
            get => _nightColorTemperature;
            set
            {
                _nightColorTemperature = value;
                OnPropertyChanged();
            }
        }
        public float NightBrightness
        {
            get => _nightBrightness;
            set
            {
                _nightBrightness = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}