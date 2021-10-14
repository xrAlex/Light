using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace Light.Infrastructure
{
    public static class UserNotifier
    {
        private static readonly TaskbarIcon Notifier = (TaskbarIcon)Application.Current.FindResource("TaskbarIcon");
        public static void ShowTip(string localizationKey)
        {
            var tipText= Localization.LangDictionary.GetString(localizationKey);
            Notifier.ShowBalloonTip(AppDomain.CurrentDomain.FriendlyName, tipText, BalloonIcon.Info);
        }
    }
}
