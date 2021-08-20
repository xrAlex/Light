using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Services
{
    public class ServiceLocator
    {
        private GammaRegulatorService _gammaRegulator;
        private SettingsService _settings;
        private WindowService _windowService;

        public GammaRegulatorService GammaRegulator 
        {
            get 
            {
                return _gammaRegulator ??= new();
            }
        }
        public SettingsService Settings 
        { 
            get 
            {
                return _settings ??= new();
            }
        }
        public WindowService WindowService 
        { 
            get
            {
                return _windowService ??= new();
            }
        }

        private ServiceLocator() {}
        private static readonly Lazy<ServiceLocator> lazy = new(() => new ServiceLocator());
        public static ServiceLocator Source { get { return lazy.Value; } }
    }
}
