using Light.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
namespace Light.Models.Entities
{
    public class ProcessEntity
    {
        private readonly ProcessBounds _processBounds = new();
        public Process Instance { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool OnFullScreen => _processBounds.IsProcessOnFullScreen(Instance);
        private string DispalyedText => $"{Name} {(Instance != null ? "" : "[N/A]")} {(OnFullScreen ? "[FullScreen]" : "")}";
        public override string ToString() => DispalyedText;

        public ProcessEntity(Process instance = null, string name = "")
        {
            if (instance == null)
            {
                Instance = TryFindProcessInstance(name);
            }
            else
            {
                Instance = instance;
            }
            _processBounds = new();
            Name = name;
        }

        private Process TryFindProcessInstance(string processName)
        {
            var processes = Process.GetProcesses();

            foreach (var Process in processes)
            {
                if (Process.ProcessName == processName && Process.MainWindowHandle != IntPtr.Zero)
                {
                    return Process;
                }
            }
            return null;
        }
    }
}

