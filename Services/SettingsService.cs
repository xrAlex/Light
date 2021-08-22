
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

            for (int i = 0; i < Screens.Count; i++)
            {
                var Monitor = Screens[i];
                manager.WriteValue($"{i}", "UserGamma", Monitor.UserGamma.ToString());
                manager.WriteValue($"{i}", "UserBlueReduce", Monitor.UserBlueReduce.ToString());
                manager.WriteValue($"{i}", "HourStart", Monitor.HourStart.ToString());
                manager.WriteValue($"{i}", "MinStart", Monitor.MinStart.ToString());
                manager.WriteValue($"{i}", "HourEnd", Monitor.HourEnd.ToString());
                manager.WriteValue($"{i}", "MinEnd", Monitor.MinEnd.ToString());
                manager.WriteValue($"{i}", "Active", Monitor.IsActive.ToString());
                manager.WriteValue($"{i}", "Name", Monitor.Name);
                manager.WriteValue($"{i}", "Sys", Monitor.SysName);
            }
        }

        public void Load()
        {
            var manager = new INIManager();

            int index = 0;
            foreach (var screen in Screen.AllScreens)
            {
                Screens.Add(new ScreenModel
                {
                    UserGamma = manager.GetFloatValue($"{index}", "UserGamma", "100"),
                    UserBlueReduce = manager.GetFloatValue($"{index}", "UserBlueReduce", "100"),
                    HourStart = manager.GetIntValue($"{index}", "HourStart", "23"),
                    MinStart = manager.GetIntValue($"{index}", "MinStart", "00"),
                    HourEnd = manager.GetIntValue($"{index}", "HourEnd", "07"),
                    MinEnd = manager.GetIntValue($"{index}", "MinEnd", "00"),
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
