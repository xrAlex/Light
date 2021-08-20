using Light.Infrastructure;
using Light.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Models
{
    public class ScreenModel : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Values

        private string _name = "";
        private string _sysName = "";
        private bool _active = false;
        private int _startHour = 23;
        private int _startMin = 10;
        private int _endHour = 7;
        private int _endMin = 10;
        private float _currentGamma = 100;
        private float _currentBlueReduce = 100;
        private float _userGamma = 100;
        private float _userBlueReduce = 100;
        private float _uiOpacity = 1f;

        #endregion

        #region Constructors

        public string SysName
        {
            get => _sysName;
            set
            {
                _sysName = value;
                RaisePropertyChanged("SysName");
            }
        }

        public float Opacity
        {
            get => _uiOpacity;
            set
            {
                _uiOpacity = value;
                RaisePropertyChanged("Opacity");
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        public bool IsActive
        {
            get => _active;
            set
            {
                _active = value;
                Opacity = value ? 1f : 0.5f;
                RaisePropertyChanged("IsActive");
            }
        }
        public int HourStart
        {
            get => _startHour;
            set
            {
                _startHour = value;
                RaisePropertyChanged("HourStart");
            }
        }
        public int MinStart
        {
            get => _startMin;
            set
            {
                _startMin = value;
                RaisePropertyChanged("MinStart");
            }
        }
        public int HourEnd
        {
            get => _endHour;
            set
            {
                _endHour = value;
                RaisePropertyChanged("HourEnd");
            }
        }
        public int MinEnd
        {
            get => _endMin;
            set
            {
                _endMin = value;
                RaisePropertyChanged("MinEnd");
            }
        }
        public float CurrentGamma
        {
            get => _currentGamma;
            set
            {
                _currentGamma = value;
                RaisePropertyChanged("CurrentGamma");
            }
        }
        public float CurrentBlueReduce
        {
            get => _currentBlueReduce;
            set
            {
                _currentBlueReduce = value;
                RaisePropertyChanged("CurrentBlueReduce");
            }
        }

        public float UserGamma
        {
            get => _userGamma;
            set
            {
                _userGamma = value;
                RaisePropertyChanged("UserGamma");
            }
        }
        public float UserBlueReduce
        {
            get => _userBlueReduce;
            set
            {
                _userBlueReduce = value;
                RaisePropertyChanged("UserBlueReduce");
            }
        }

        #endregion

        #region Methods
        public override string ToString() => SysName;
        

        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
