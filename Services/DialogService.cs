#region

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Light.Templates.Entities;
using Light.ViewModels.Base;

#endregion

namespace Light.Services
{
    /// <summary>
    /// Class for working with program dialog windows
    /// </summary>
    internal sealed class DialogService
    {
        private readonly Dictionary<Type, DialogStorage> _mappings;

        /// <summary>
        /// Registers View Model and View types for further instantiation
        /// </summary>
        public void Register<TViewModel, TView>() where TViewModel : ViewModelBase where TView : Window
        {
            _mappings.Add(typeof(TViewModel), new DialogStorage(typeof(TView)));
        }

        /// <summary>
        /// Method displays a dialog window, if the window is not instantiated, then it creates it
        /// </summary>
        public void ShowDialog<TViewModel>(Type owner = null) where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialog = dialogStorage.Instance;
            if (dialog == null)
            {
                dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
                dialogStorage.Instance = dialog;
                if (owner != null)
                {
                    var dialogOwner = _mappings[owner];
                    dialog.Owner = dialogOwner?.Instance;
                }
            }
            dialogStorage.ShowInstance();
        }

        /// <summary>
        /// Closing dialog window
        /// </summary>
        public void CloseDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            dialogStorage.CloseInstance();
        }

        /// <summary>
        /// Hiding dialog window
        /// </summary>
        public void HideDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            dialogStorage.HideInstance();
        }

        public DialogService()
        {
            _mappings = new();
        }

#if DEBUG
        ~DialogService()
        {
            DebugConsole.Print("[Service] DialogService Disposed");
        }
#endif
    }
}
