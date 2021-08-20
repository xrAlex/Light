using Light.Infrastructure;
using Light.Services;
using Light.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Light
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
            var serviceLocator = ServiceLocator.Source;
            var appSettings = serviceLocator.Settings;
            appSettings.Load();
        }
    }
}
