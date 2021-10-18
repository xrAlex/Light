using System;
using Light.Services.Interfaces;
using Ninject;

namespace Light.Localization
{
    public partial class LangDictionary
    {
        public static event EventHandler<EventArgs> OnLocalizationChanged;
        public static LangDictionary Instance { get; private set; } = new();

        public LangDictionary()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeComponent();
                var selectedLang = App
                    .Kernel
                    .Get<ISettingsService>()
                    .SelectedLang;
                SetLang(selectedLang);
            }
        }

        public static void SetLang(int langIndex)
        {
            switch (langIndex)
            {
                case 0:
                    Eng();
                    break;
                case 1:
                    Rus();
                    break;
                case 2:
                    Chn();
                    break;
            }

            OnLocalizationChanged?.Invoke(null, EventArgs.Empty);
        }

        public static string GetString(string param)
        {
            return Instance[$"{param}"] != null ? Instance[$"{param}"].ToString() : "Localization error";
        }

        private static void Rus()
        {
            var dict = Instance;

            dict
                ["Loc_Apply"] = "Применить";
            dict
                ["Loc_AutoLaunch"] = "Запускаться вместе с Windows";
            dict
                ["Loc_Brightness"] = "Яркость";
            dict
                ["Loc_Cancel"] = "Отменить";
            dict
                ["Loc_ColorTemperature"] = "Цветовая температура";
            dict
                ["Loc_Day"] = "День";
            dict
                ["Loc_DontWorkInFullScreen"] = "Не работать в полноэкранных приложениях";
            dict
                ["Loc_ExtendedGammaRange"] = "Расширенный диапозон гаммы";
            dict
                ["Loc_Monitors"] = "Мониторы";
            dict
                ["Loc_Night"] = "Ночь";
            dict
                ["Loc_ProcessesWhiteList"] = "Исключения";
            dict
                ["Loc_Reset"] = "Сбросить";
            dict
                ["Loc_RestartNotification"] = "Для применения параметров требуется перезагрузка";
            dict
                ["Loc_SmoothBrightnessChange"] = "Плавное изменение яркости";
            dict
                ["Loc_Sunrise"] = "Восход";
            dict
                ["Loc_Sunset"] = "Закат";
            dict
                ["Loc_ToTrayNotification"] = "Приложение продолжит работу в свернутом состоянии";
            dict
                ["Loc_TrayClose"] = "Закрыть";
            dict
                ["Loc_TrayPause"] = "Приостановить";
            dict
                ["Loc_TrayUnPause"] = "Продолжить";
        }

        private static void Eng()
        {
            var dict = Instance;

            dict
                ["Loc_Apply"] = "Apply";
            dict
                ["Loc_AutoLaunch"] = "Start with Windows";
            dict
                ["Loc_Brightness"] = "Brightness";
            dict
                ["Loc_Cancel"] = "Cancel";
            dict
                ["Loc_ColorTemperature"] = "Color temperature";
            dict
                ["Loc_Day"] = "Day";
            dict
                ["Loc_DontWorkInFullScreen"] = "Dont work in full screen apllications";
            dict
                ["Loc_ExtendedGammaRange"] = "Extended gamma range";
            dict
                ["Loc_Monitors"] = "Monitors";
            dict
                ["Loc_Night"] = "Night";
            dict
                ["Loc_ProcessesWhiteList"] = "Applications whitelist";
            dict
                ["Loc_Reset"] = "Reset";
            dict
                ["Loc_RestartNotification"] = "Restart required to apply parameters";
            dict
                ["Loc_SmoothBrightnessChange"] = "Smooth brightness change";
            dict
                ["Loc_Sunrise"] = "Sunrise";
            dict
                ["Loc_Sunset"] = "Sunset";
            dict
                ["Loc_ToTrayNotification"] = "Application will continue to work in a collapsed state";
            dict
                ["Loc_TrayClose"] = "Close";
            dict
                ["Loc_TrayPause"] = "Pause";
            dict
                ["Loc_TrayUnPause"] = "Unpause";
        }

        private static void Chn()
        {
            var dict = Instance;

            dict
                ["Loc_Apply"] = "申请";
            dict
                ["Loc_AutoLaunch"] = "与 Windows 同时运行应用程序";
            dict
                ["Loc_Brightness"] = "亮度";
            dict
                ["Loc_Cancel"] = "取消";
            dict
                ["Loc_ColorTemperature"] = "色温";
            dict
                ["Loc_Day"] = "日";
            dict
                ["Loc_DontWorkInFullScreen"] = "不要在全屏应用程序中工作";
            dict
                ["Loc_ExtendedGammaRange"] = "扩展伽马范围";
            dict
                ["Loc_Monitors"] = "监视器";
            dict
                ["Loc_Night"] = "夜晚";
            dict
                ["Loc_ProcessesWhiteList"] = "应用白名单";
            dict
                ["Loc_Reset"] = "重启";
            dict
                ["Loc_RestartNotification"] = "需要重新启动系统才能应用应用程序设置";
            dict
                ["Loc_SmoothBrightnessChange"] = "平滑的亮度变化";
            dict
                ["Loc_Sunrise"] = "日出";
            dict
                ["Loc_Sunset"] = "日落";
            dict
                ["Loc_ToTrayNotification"] = "应用程序将在折叠状态下继续工作";
            dict
                ["Loc_TrayClose"] = "关闭";
            dict
                ["Loc_TrayPause"] = "暂停";
            dict
                ["Loc_TrayUnPause"] = "取消暂停";
        }
    }
}
