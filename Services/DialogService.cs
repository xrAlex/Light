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
    /// Класс для работы с диалоговыми окнами программы
    /// </summary>
    public class DialogService
    {
        private readonly Dictionary<Type, DialogStorage> _mappings;

        /// <summary>
        /// Метод регестрирует типы ViewModel и View для дальнейшего создания экземпляра
        /// </summary>
        public void Register<TViewModel, TView>(bool isTrayMenu = false) where TViewModel : ViewModelBase where TView : Window
        {
            _mappings.Add(typeof(TViewModel), new DialogStorage(typeof(TView), isTrayMenu));
        }

        /// <summary>
        /// Метод отображает диалоговое окно, если экземпляр окна не создан, то создает его
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
        /// Метод закрывает диалоговое окно
        /// </summary>
        public void CloseDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            dialogStorage.CloseInstance();
        }

        /// <summary>
        /// Метод скрывает диалоговое окно
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
    }
}
