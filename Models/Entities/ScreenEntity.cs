using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Light.Models.Entities
{
    public class ScreenEntity : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Values

        private bool _active = false;
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
            set
            {
                _uiOpacity = value;
                RaisePropertyChanged("Opacity");
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
        public int StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                RaisePropertyChanged("HourStart");
            }
        }

        public int EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                RaisePropertyChanged("EndTime");
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
