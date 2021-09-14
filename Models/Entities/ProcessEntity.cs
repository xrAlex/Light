using System;
using System.Diagnostics;
using System.Linq;
using Light.Infrastructure;

namespace Light.Models.Entities
{
    public class ProcessEntity
    {
        private readonly ProcessBounds _processBounds;

        public Process Instance { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        private bool OnFullScreen => _processBounds.IsProcessOnFullScreen(Instance);
        private string DispalyedText => $"{Name} {(Instance != null ? "" : "[N/A]")} {(OnFullScreen ? "[FullScreen]" : "")}";

        public override string ToString()
        {
            return DispalyedText;
        }

        private Process TryFindProcessInstance(string processName)
        {
            var processes = Process.GetProcesses();

            return processes.FirstOrDefault(process => process.ProcessName == processName && process.MainWindowHandle != IntPtr.Zero);
        }

        public ProcessEntity(Process instance = null, string name = "")
        {
            Instance = instance ?? TryFindProcessInstance(name);
            _processBounds = new ProcessBounds();
            Name = name;
        }

    }
}