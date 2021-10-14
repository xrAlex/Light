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
#if DEBUG
            else
            {
                Logging.Write($"Instance is null, Type: [{Type}]");
            }
#endif
        }

        public void ShowInstance()
        {
            if (Instance != null)
            {
                Instance.Show();
                Instance.Activate();
            }
#if DEBUG
            else
            {
                Logging.Write($"Instance is null, Type: [{Type}]");
            }
#endif
        }
        public DialogStorage(Type type)
        {
            Type = type;
        }
    }
}
