using Light.Infrastructure;
using Light.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Infrastructure
{
    class FindIndexByName
    {
        public int GetIndex(string name)
        {
            var serviceLocator = ServiceLocator.Source;
            var settings = serviceLocator.Settings;
            var monitorsDictonary = settings.Screens;

            var elem = monitorsDictonary.FirstOrDefault(x => string.Equals(x.SysName, name, StringComparison.InvariantCultureIgnoreCase));
            return monitorsDictonary.IndexOf(elem);
        }
    }
}
