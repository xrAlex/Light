using System;
using System.Windows;
using System.Windows.Input;
using Light.Infrastructure;

namespace Light.Templates.Entities
{
    internal sealed class DialogStorage
    {
        public Type Type { get; }
        public Window Instance { get; set; }

        public void CloseInstance()
        {
            if (Instance != null)
            {
                Instance.Close();
                Instance = null;
            }
        }

        public void ShowInstance()
        {
            if (Instance != null)
            {
                Instance.Show();
                Instance.Activate();
            }
        }

        public void HideInstance() => Instance.Hide();

        public DialogStorage(Type type)
        {
            Type = type;
        }
    }
}
