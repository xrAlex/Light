#region

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Light.Infrastructure;
using Light.Services;
using Light.Templates.Entities;

#endregion

namespace Light.Models
{
    public class ProcessModel
    {
        #region Fields

        public ObservableCollection<ProcessEntity> Processes { get; }
        private ObservableCollection<ProcessEntity> IgnoredProcesses { get; }

        #endregion

        #region Methods

        public void MoveToIgnoredProcesses()
        {
            IgnoredProcesses.Clear();
            Processes
                .Where(x => x.IsSelected)
                .ToList()
                .ForEach(x =>
                {
                    IgnoredProcesses.Add(new ProcessEntity(x.Instance, x.Name));
                });
        }

        private void FillProcessCollection()
        {
            Process.GetProcesses()
                .AsParallel()
                .Where(x => WindowHelper.IsWindowValid(x.MainWindowHandle))
                .ForAll(x =>
                {
                    Processes.Add
                    (
                        new ProcessEntity(x, x.ProcessName)
                        {
                            IsSelected = IgnoredProcesses.Any(y => y.Name == x.ProcessName)
                        }
                    );
                });
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