using System;
using System.Windows;
using System.Windows.Input;
using Light.Infrastructure;

namespace Light.Templates.Entities
{
    public class DialogStorage
    {
        private readonly bool _isTrayMenu;
        private Window _instance;
        public Type Type { get; }

        public Window Instance
        {
            get => _instance;
            set
            {
                _instance = value;

                if (_instance != null)
                {
                    if (_isTrayMenu)
                    {
                        _instance.Deactivated += InstanceOnDeactivated;
                    }
                    else
                    {
                        _instance.MouseLeftButtonDown += InstanceOnMouseLeftButtonDown;
                    }
                }
            }
        }

        private void InstanceOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Instance.DragMove();

        private void InstanceOnDeactivated(object sender, EventArgs e)
        {
            CloseInstance();
        }

        public void CloseInstance()
        {
            Instance?.Close();
        }

        public void ShowInstance()
        {
            Instance?.Show();
            Instance?.Activate();
        }

        public void HideInstance() => Instance.Hide();

        public DialogStorage(Type type = null, bool isTrayMenu = false)
        {
            Type = type;
            _isTrayMenu = isTrayMenu;
        }
    }
}
