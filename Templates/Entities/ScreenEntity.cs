#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

#endregion

namespace Light.Templates.Entities
{
    public sealed class ScreenEntity : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        public readonly ColorConfiguration ColorConfiguration = new();

        private readonly List<BitmapImage> _imageList = new()
        {
            new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Gamma.png", UriKind.Absolute)),
            new BitmapImage(new Uri("pack://application:,,,/Resources/Images/BlueReduce.png", UriKind.Absolute))
        };

        #endregion

        #region Values

        private bool _active;
        private int _startTime;
        private int _endTime;
        private float _uiOpacity;
        private bool _isDayTimePeriod;
        private BitmapImage _currentTimePeriodImage;

        #endregion

        #region Propeties

        public Dictionary<string,int> Bounds { get; set; }
        public string SysName { get; set; }
        public string Name { get; set; }

        public ColorConfiguration GetColorConfiguration => ColorConfiguration;

        public BitmapImage CurrentTimePeriodImage
        {
            get => _currentTimePeriodImage;
            private set
            {
                _currentTimePeriodImage = value;
                OnPropertyChanged();
            }
        }

        public bool IsDayTimePeriod
        {
            get => _isDayTimePeriod;
            set
            {
                _isDayTimePeriod = value;
                CurrentTimePeriodImage = value ? _imageList[0] : _imageList[1];
            }
        }

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

        #endregion

        #region Methods

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}