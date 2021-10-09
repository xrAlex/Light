using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Localization
{
    public partial class LangDictionary
    {
        public static LangDictionary Instance { get; } = new();

        public LangDictionary()
        {
            Eng(this); // default
        }

        public static string GetString(string param)
        {
            return Instance[$"{param}"] != null ? Instance[$"{param}"].ToString() : "Localization error";
        }

        public static void Rus(LangDictionary dict = null)
        {
            dict ??= Instance;

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
                ["Loc_ExtendedGammaRange"] = "Активирвоать расширенный диапозон гаммы";
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
                ["Loc_TrayUnpause"] = "Продолжить";
        }
        public static void Eng(LangDictionary dict = null)
        {
            dict ??= Instance;

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
                ["Loc_ExtendedGammaRange"] = "Enable extended gamma range";
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
                ["Loc_TrayUnpause"] = "Unpause";
        }
    }
}
