using Sparky.Templates.Entities.Base;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Sparky.Templates.Entities
{
    internal sealed class ScreenEntity : NotifyingEntity
    {
        #region Fields

        private BitmapImage _currentTimePeriodImage;
        private ColorConfiguration _dayColorConfiguration;
        private ColorConfiguration _nightColorConfiguration;
        private ColorConfiguration _currentColorConfiguration;
        private StartTime _nightStartTime;
        private StartTime _dayStartTime;
        private float _uiOpacity;
        private bool _active;
        private bool _isDayTimePeriod;

        private readonly List<BitmapImage> _imageList = new()
        {
            new BitmapImage(new Uri("/Resources/Images/Gamma.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Resources/Images/BlueReduce.png", UriKind.Relative))
        };

        #endregion

        #region Propeties

        public string SysName { get; set; }
        public string Name { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int DisplayCode { get; set; }
        public nint DeviceContext { get; set; }

        public ColorConfiguration DayColorConfiguration
        {
            get => _dayColorConfiguration;
            set
            {
                _dayColorConfiguration = value;
                OnPropertyChanged();
            }
        }

        public ColorConfiguration NightColorConfiguration
        {
            get => _nightColorConfiguration;
            set
            {
                _nightColorConfiguration = value;
                OnPropertyChanged();
            }
        }

        public ColorConfiguration CurrentColorConfiguration
        {
            get => _currentColorConfiguration;
            set
            {
                _currentColorConfiguration = value;
                OnPropertyChanged();
            }
        }

        public StartTime DayStartTime
        {
            get => _dayStartTime;
            set
            {
                _dayStartTime = value;
                OnPropertyChanged();
            }
        }

        public StartTime NightStartTime
        {
            get => _nightStartTime;
            set
            {
                _nightStartTime = value;
                OnPropertyChanged();
            }
        }

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
        #endregion
    }
}