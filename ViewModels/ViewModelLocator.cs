using Microsoft.Extensions.DependencyInjection;

namespace Light.ViewModels
{
    internal sealed class ViewModelLocator
    {
        public static MainWindowViewModel MainWindowViewModel => App.ServicesHost.Services.GetRequiredService<MainWindowViewModel>();
        public static OtherSettingsPageViewModel OtherSettingsPageViewModel => App.ServicesHost.Services.GetRequiredService<OtherSettingsPageViewModel>();
        public static ProcessPageViewModel ProcessPageViewModel => App.ServicesHost.Services.GetRequiredService<ProcessPageViewModel>();
        public static SettingsMainPageViewModel SettingsMainPageViewModel => App.ServicesHost.Services.GetRequiredService<SettingsMainPageViewModel>();
        public static SettingsWindowViewModel SettingsWindowViewModel => App.ServicesHost.Services.GetRequiredService<SettingsWindowViewModel>();
        public static TrayMenuViewModel TrayMenuViewModel => App.ServicesHost.Services.GetRequiredService<TrayMenuViewModel>();
    }
}
