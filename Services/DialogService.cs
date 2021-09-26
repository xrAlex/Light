#region

using System;
using System.Collections.Generic;
using System.Windows;
using Light.ViewModels.Base;

#endregion

namespace Light.Services
{
    public class DialogStorage
    {
        public Window Instance { get; set; }
        public Window Owner { get; }
        public Type Type { get; }

        public void CloseInstance()
        {
            Instance.Close();
            Instance = null;
        }

        public DialogStorage(Window instance = null, Window owner = null, Type type = null)
        {
            Instance = instance;
            Owner = owner;
            Type = type;
        }
    }
    public class DialogService
    {
        private readonly Dictionary<Type, DialogStorage> _mappings;

        public void Register<TViewModel, TView>(Window owner = null) where TViewModel : ViewModelBase where TView : Window
        {
            _mappings.Add(typeof(TViewModel), new DialogStorage(null, owner, typeof(TView)));
        }

        public void ShowDialog<TViewModel, TOwner>() where TViewModel : ViewModelBase where TOwner : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialogOwner = _mappings[typeof(TOwner)];
            var dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
            dialogStorage.Instance = dialog;
            dialog.Owner = dialogOwner.Instance;
            dialog.ShowDialog();
        }

        public void ShowDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            var dialogStorage = _mappings[typeof(TViewModel)];
            var dialog = (Window)Activator.CreateInstance(dialogStorage.Type);
            dialogStorage.Instance = dialog;
            dialog.ShowDialog();
        }

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
