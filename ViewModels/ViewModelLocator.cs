using System.ComponentModel;
using System.Windows;
using Ninject;

namespace Sparky.ViewModels
{
    internal sealed class ViewModelLocator
    {
        private static readonly DependencyObject _dependencyObject = new ();

        public static MainWindowViewModel MainWindowViewModel => 
            IsInDesignMode() 
                ? new MainWindowViewModel() 
                : App.Kernel.Get<MainWindowViewModel>();

        public static OtherSettingsPageViewModel OtherSettingsPageViewModel => 
            IsInDesignMode() 
                ?  new OtherSettingsPageViewModel() 
                : App.Kernel.Get<OtherSettingsPageViewModel>();

        public static ProcessPageViewModel ProcessPageViewModel => 
            IsInDesignMode() 
                ? new ProcessPageViewModel() 
                : App.Kernel.Get<ProcessPageViewModel>();

        public static SettingsMainPageViewModel SettingsMainPageViewModel => 
            IsInDesignMode() 
                ? new SettingsMainPageViewModel() 
                : App.Kernel.Get<SettingsMainPageViewModel>();

        public static SettingsWindowViewModel SettingsWindowViewModel =>
            IsInDesignMode()
                ? new SettingsWindowViewModel()
                : App.Kernel.Get<SettingsWindowViewModel>();
        public static TrayMenuViewModel TrayMenuViewModel =>
            IsInDesignMode()
                ? new TrayMenuViewModel()
                : App.Kernel.Get<TrayMenuViewModel>();

        public static InformationViewModel InformationViewModel =>
            IsInDesignMode()
                ? new InformationViewModel()
                : App.Kernel.Get<InformationViewModel>();

        private static bool IsInDesignMode() => DesignerProperties.GetIsInDesignMode(_dependencyObject);
    }
}
