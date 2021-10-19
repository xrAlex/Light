using System;
using System.Windows;
using Sparky.ViewModels.Base;

namespace Sparky.Services.Interfaces
{
    internal interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : ViewModelBase where TView : Window;
        void ShowDialog<TViewModel>(Type owner = null) where TViewModel : ViewModelBase;
        void CloseDialog<TViewModel>() where TViewModel : ViewModelBase;
        void HideDialog<TViewModel>() where TViewModel : ViewModelBase;
    }
}