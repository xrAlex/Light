using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Light.Infrastructure;
using Light.Models;
using Light.Native;

namespace Light.Services
{
    public class ColorTemperatureWatcherService
    {
        private readonly ScreenModel _screenModel;
        private readonly SettingsService _settings;
        private readonly WorkTime _workTime;
        private CancellationTokenSource _cts;

        public ColorTemperatureWatcherService()
        {
            _screenModel = new ScreenModel();
            _workTime = new WorkTime();
            var serviceLocator = ServiceLocator.Source;
            _settings = serviceLocator.Settings;
        }

        public void StartWatch()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => Cycle(_cts.Token), _cts.Token);
        }

        public void StopWatch()
        {
            _cts.Cancel();
        }

        private async Task Cycle(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var screen in _screenModel.Screens)
                {
                    if (!screen.IsActive) continue;

                    var isWorkTime = _workTime.IsNightTemperatureTime(screen);

                    if (isWorkTime)
                    {
                        if (_settings.CheckFullScreenApps)
                        {
                            if (IsFullScreenProcessFounded(screen.Instance))
                            {
                                _screenModel.SetDayTemperature(screen);
                                break;
                            }
                        }
                        _screenModel.SetNightTemperature(screen);
                        break;
                    }

                    _screenModel.SetDayTemperature(screen);
                }

                await Task.Delay(1000, token);
            }
        }

        private bool IsFullScreenProcessFounded(Screen screen)
        {
            var handler = User32.GetForegroundWindow();
            if (!WindowHelper.IsWindowValid(handler) || !WindowHelper.IsWindowOnFullScreen(screen, handler)) return false;

            uint pid = 0;
            User32.GetWindowThreadProcessId(handler, ref pid);
            if (pid == 0) return false;

            var process = Process.GetProcessById((int)pid);
            return _settings.IgnoredProcesses.Any(p => p.Name != process.ProcessName);
        }

    }
}