using System;
using System.Windows;

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
        public DialogStorage(Type type)
        {
            Type = type;
        }
    }
}
