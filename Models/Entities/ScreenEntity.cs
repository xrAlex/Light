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
        private float _currentGamma = 100;
        private float _currentBlueReduce = 100;
        private float _userGamma = 100;
        private float _userBlueReduce = 100;
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

        public float CurrentGamma
        {
            get => _currentGamma;
            set
            {
                _currentGamma = value;
                OnPropertyChanged();
            }
        }

        public float CurrentBlueReduce
        {
            get => _currentBlueReduce;
            set
            {
                _currentBlueReduce = value;
                OnPropertyChanged();
            }
        }

        public float UserGamma
        {
            get => _userGamma;
            set
            {
                _userGamma = value;
                OnPropertyChanged();
            }
        }

        public float UserBlueReduce
        {
            get => _userBlueReduce;
            set
            {
                _userBlueReduce = value;
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