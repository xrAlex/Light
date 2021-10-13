using Light.Templates.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Light.Services.Interfaces
{
    internal interface ISettingsService
    {
        ObservableCollection<ApplicationEntity> Applications { get; }
        bool CheckFullScreenApps { get; set; }
        List<string> ApplicationsWhitelist { get; }
        ObservableCollection<ScreenEntity> Screens { get; }
        int SelectedLang { get; set; }
        int SelectedScreen { get; set; }

        void Load();
        void Reset();
        void Save();
    }
}