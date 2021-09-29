#region

using System;
using System.Collections.Generic;
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
        public void Register<TViewModel, TView>(Window owner = null) where TViewModel : ViewModelBase where TView : Window
        {
            _mappings.Add(typeof(TViewModel), new DialogStorage(null, owner, typeof(TView)));
        }

        /// <summary>
        /// Метод создает экземпляр диалогового окна и отображает его, принимает вторым параметром Owner окна
        /// </summary>
        public void CreateDialog<TViewModel, TOwner>() where TViewModel : ViewModelBase where TOwner : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialogOwner = _mappings[typeof(TOwner)];
            var dialog = dialogStorage.Instance;
            if (dialog == null)
            {
                dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
                dialogStorage.Instance = dialog;
            }
            dialog.Owner = dialogOwner.Instance;
        }

        /// <summary>
        /// Метод создает экземпляр диалогового окна
        /// </summary>
        public void CreateDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialog = dialogStorage.Instance;
            if (dialog == null)
            {
                dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
                dialogStorage.Instance = dialog;
            }
        }

        /// <summary>
        /// Метод отображает диалогового окна, если экземпляр окна не создан, то создает его
        /// </summary>
        public void ShowDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialog = dialogStorage.Instance;
            if (dialog == null)
            {
                dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
                dialogStorage.Instance = dialog;
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
