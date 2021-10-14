using Ninject;

namespace Light.ViewModels
{
    internal sealed class ViewModelLocator
    {
        public static MainWindowViewModel MainWindowViewModel => App.Kernel.Get<MainWindowViewModel>();
        public static OtherSettingsPageViewModel OtherSettingsPageViewModel => App.Kernel.Get<OtherSettingsPageViewModel>();
        public static ProcessPageViewModel ProcessPageViewModel => App.Kernel.Get<ProcessPageViewModel>();
        public static SettingsMainPageViewModel SettingsMainPageViewModel => App.Kernel.Get<SettingsMainPageViewModel>();
        public static SettingsWindowViewModel SettingsWindowViewModel => App.Kernel.Get<SettingsWindowViewModel>();
        public static TrayMenuViewModel TrayMenuViewModel => App.Kernel.Get<TrayMenuViewModel>();
    }
}
