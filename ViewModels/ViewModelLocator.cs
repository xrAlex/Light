using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowVM { get; set; }
        public OtherSettingsPageViewModel OtherSettingsPageVM { get; set; }
        public ProcessPageViewModel ProcessPageVM { get; set; }
        public SettingsMainPageViewModel SettingsMainPageVM { get; set; }
        public SettingsWindowViewModel SettingsWindowVM { get; set; }

        private ViewModelLocator() {}
        private static readonly Lazy<ViewModelLocator> lazy = new(() => new ViewModelLocator());
        public static ViewModelLocator Source { get { return lazy.Value; } }
    }
}
