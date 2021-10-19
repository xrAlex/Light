using System;
using System.Windows;

namespace Sparky.Templates.Entities
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
                LoggingModule.Log.Information("[CloseInstance] Instance is null, Type: [{0}]",Type);
            }
#endif
        }

        public void ShowInstance()
        {
            if (Instance != null)
            {
                Instance.Show();
                Instance.Activate();
                Instance.Focus();
            }
#if DEBUG
            else
            {
                LoggingModule.Log.Information("[ShowInstance] Instance is null, Type: [{0}]", Type);
            }
#endif
        }

        public void HideInstance()
        {
            if (Instance != null)
            {
                Instance.Hide();
            }
#if DEBUG
            else
            {
                LoggingModule.Log.Information("[HideInstance] Instance is null, Type: [{0}]", Type);
            }
#endif
        }
        public DialogStorage(Type type)
        {
            Type = type;
        }
    }
}
