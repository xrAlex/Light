using Light.Templates.Entities.Base;

namespace Light.Templates.Entities
{
    internal sealed class ColorConfiguration : NotifyingEntity
    {
        #region Fields

        private double _currentBrightness;
        private int _currentColorTemperature;
        private int _dayColorTemperature;
        private double _dayBrightness;
        private int _nightColorTemperature;
        private double _nightBrightness;

        #endregion

        #region Constructors

        public double CurrentBrightness
        {
            get => _currentBrightness;
            set
            {
                _currentBrightness = value;
                OnPropertyChanged();
            }
        }

        public int CurrentColorTemperature
        {
            get => _currentColorTemperature;
            set
            {
                _currentColorTemperature = value;
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

        public double DayBrightness
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

        public double NightBrightness
        {
            get => _nightBrightness;
            set
            {
                _nightBrightness = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
