using Light.Templates.Entities.Base;

namespace Light.Templates.Entities
{
    public class ColorConfiguration : NotifyingEntity
    {
        #region Fields

        private float _currentBrightness;
        private int _currentColorTemperature;
        private int _dayColorTemperature;
        private float _dayBrightness;
        private int _nightColorTemperature;
        private float _nightBrightness;

        #endregion

        #region Constructors

        public float CurrentBrightness
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
    }
}
