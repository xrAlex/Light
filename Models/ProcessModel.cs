using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Models.Entities;
using Light.Native;
using Light.Services;

namespace Light.Models
{
    public class ProcessModel
    {
        #region Fields

        public ObservableCollection<ProcessEntity> Processes { get; }
        public ObservableCollection<ProcessEntity> IgnoredProcesses { get; }

        #endregion

        #region Methods

        public void MoveToIgnoredProcesses()
        {
            Processes
                .Where(x => x.IsSelected)
                .ToList()
                .ForEach(x =>
                {
                    IgnoredProcesses.Add(new ProcessEntity(x.Instance, x.Name));
                    Processes.Remove(x);
                });
        }

        public void MoveToProcesses()
        {
            IgnoredProcesses
                .Where(x => x.IsSelected)
                .ToList()
                .ForEach(x =>
                {
                    Processes.Add(new ProcessEntity(x.Instance, x.Name));
                    IgnoredProcesses.Remove(x);
                });
        }

        private void FillProcessCollection()
        {
            Process.GetProcesses()
                .AsParallel()
                .Where(x => WindowHelper.IsWindowValid(x.MainWindowHandle) && !IgnoredProcesses.Any(y => y.Name == x.ProcessName))
                .ForAll(x => Processes.Add(new ProcessEntity(x, x.ProcessName)));
        }


        #endregion

        public ProcessModel()
        {
            var serviceLocator = ServiceLocator.Source;
            var settingsService = serviceLocator.Settings;
            IgnoredProcesses = settingsService.IgnoredProcesses;
            Processes = new ObservableCollection<ProcessEntity>();
            FillProcessCollection();
        }
    }
}