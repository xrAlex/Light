using Sparky.Templates.Entities.Base;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Sparky.Templates.Entities
{
    internal sealed class ScreenEntity : NotifyingEntity
    {
        #region Fields

        private bool _active;
        private int _startTime;
        private int _endTime;
        private float _uiOpacity;
        private bool _isDayTimePeriod;
        private BitmapImage _currentTimePeriodImage;

        public ColorConfiguration ColorConfiguration { get; } = new();

        private readonly List<BitmapImage> _imageList = new()
        {
            new BitmapImage(new Uri("/Resources/Images/Gamma.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/Images/Gamma.png", UriKind.Relative))
        };

        #endregion

        #region Propeties
        public string SysName { get; set; }
        public string Name { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int DisplayCode { get; set; }

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
    }
}