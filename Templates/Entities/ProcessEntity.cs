#region

using System.Diagnostics;
using System.Linq;
using Light.Infrastructure;

#endregion

namespace Light.Templates.Entities
{
    public class ProcessEntity
    {
        public Process Instance { get; }
        public string Name { get; }
        public bool IsSelected { get; set; }
        private bool OnFullScreen { get; }
        public string DisplayedText => $"{Name} {(Instance != null ? "" : "[N/A]")} {(OnFullScreen ? "[FullScreen]" : "")}";

        private Process TryFindProcessInstance(string processName) => Process.GetProcesses().FirstOrDefault(process => process.ProcessName == processName);
        
        private bool IsFullScreenProcess()
        {
            if (Instance == null) return false;
            var serviceLocator = Services.ServiceLocator.Source;
            var settings = serviceLocator.Settings;

            foreach (var screen in settings.Screens)
            {
                var handle = Instance.MainWindowHandle;
                return WindowHelper.IsWindowValid(handle) && WindowHelper.IsWindowOnFullScreen(screen.Instance, handle);
            }
            return false;
        }

        public ProcessEntity(Process instance = null, string name = "")
        {
            Instance = instance ?? TryFindProcessInstance(name);
            Name = name == "" && instance != null? Instance.ProcessName : name;
            OnFullScreen = IsFullScreenProcess();
        }
    }
}