
using Light.Infrastructure;
using Light.Models;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Light.Services
{
    public sealed class SettingsService
    {
        public ObservableCollection<ScreenModel> Screens { get; set; } = new();
        public int SelectedScreen { get; set; }

        public void Save()
        {
            var manager = new INIManager();

            manager.WriteValue("Main", "SelectedScreen", SelectedScreen.ToString());

            for (int i = 0; i < Screens.Count; i++)
            {
                var Monitor = Screens[i];
                manager.WriteValue($"{i}", "UserGamma", Monitor.UserGamma.ToString());
                manager.WriteValue($"{i}", "UserBlueReduce", Monitor.UserBlueReduce.ToString());
                manager.WriteValue($"{i}", "StartTime", Monitor.StartTime.ToString());
                manager.WriteValue($"{i}", "EndTime", Monitor.EndTime.ToString());
                manager.WriteValue($"{i}", "Active", Monitor.IsActive.ToString());
                manager.WriteValue($"{i}", "Name", Monitor.Name);
                manager.WriteValue($"{i}", "Sys", Monitor.SysName);
            }
        }

        public void Load()
        {
            var manager = new INIManager();

            SelectedScreen = manager.GetIntValue("Main", "SelectedScreen", "0");

            int index = 0;
            foreach (var screen in Screen.AllScreens)
            {
                Screens.Add(new ScreenModel
                {
                    UserGamma = manager.GetFloatValue($"{index}", "UserGamma", "100"),
                    UserBlueReduce = manager.GetFloatValue($"{index}", "UserBlueReduce", "100"),
                    StartTime = manager.GetIntValue($"{index}", "StartTime", "1380"),
                    EndTime = manager.GetIntValue($"{index}", "EndTime", "420"),
                    IsActive = manager.GetBoolValue($"{index}", "Active", "true"),
                    Name = manager.GetStringValue($"{index}", "Name", $"#{ index + 1 }"),
                    SysName = manager.GetStringValue($"{index}", "SysName", $"{ screen.DeviceName }"),
                });
                index++;
            }
        }

        public void Reset()
        {
            Screens.Clear();
            Load();
        }
    }
}
