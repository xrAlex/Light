using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Models.Entities
{
    public class ProcessEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name = "";
        private bool _selected = false;
        private bool _fullScreen = false;

        public string Name 
        {
            get => _name;
            set 
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public bool IsSelected 
        {
            get => _selected;
            set 
            {
                _selected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public bool OnFullScreen 
        {
            get => _fullScreen;
            set 
            {
                _fullScreen = value;
                RaisePropertyChanged("OnFullScreen");
            } 
        }

        public override string ToString() => Name;

        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
