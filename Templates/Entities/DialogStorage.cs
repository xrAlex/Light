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
                LoggingModule.Log.Information("Instance is null, Type: [{0}]",Type);
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
                LoggingModule.Log.Information("Instance is null, Type: [{0}]", Type);
            }
#endif
        }
        public DialogStorage(Type type)
        {
            Type = type;
        }
    }
}
