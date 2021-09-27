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
        public void ShowDialog<TViewModel, TOwner>() where TViewModel : ViewModelBase where TOwner : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialogOwner = _mappings[typeof(TOwner)];
            var dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
            dialogStorage.Instance = dialog;
            dialog.Owner = dialogOwner.Instance;
            dialog.ShowDialog();
        }

        /// <summary>
        /// Метод создает экземпляр диалогового окна и отображает его
        /// </summary>
        public void ShowDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
            dialogStorage.Instance = dialog;
            dialog.ShowDialog();
        }

        /// <summary>
        /// Метод закрывает диалоговое
        /// </summary>
        public void CloseDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialog = _mappings[typeof(TViewModel)];
            dialog.CloseInstance();
        }

        public DialogService()
        {
            _mappings = new();
        }
    }
}
