using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Better_Windows_Mouse_Sensitivty.Views;

namespace Better_Windows_Mouse_Sensitivty
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            
         
        }

        public void SetLocalizationDictionary(string culture)
        {
            ResourceDictionary dict = new ResourceDictionary();
            var uri = new Uri($"pack://application:,,,/Localization/{culture}.xaml");
            try
            {
                dict.Source = uri;
            }
            catch
            {

            }
            if(dict.Count > 0)
            {
                Current.Resources.MergedDictionaries.Clear();
                Current.Resources.MergedDictionaries.Add(dict);
            }

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetLocalizationDictionary(Thread.CurrentThread.CurrentUICulture.ToString());
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
