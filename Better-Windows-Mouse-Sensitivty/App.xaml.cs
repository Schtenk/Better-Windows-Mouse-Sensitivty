﻿using System;
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
using Better_Windows_Mouse_Sensitivty.ViewModels;
using Better_Windows_Mouse_Sensitivty.Views;
using Squirrel;

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
                Current.Resources = dict;
            }

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: OnAppInstall,
                onAppUninstall: OnAppUninstall,
                onEveryRun: OnAppRun);

            CheckForUpdate();

            SetLocalizationDictionary(Thread.CurrentThread.CurrentUICulture.ToString());
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }


        private void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
        {
            tools.SetProcessAppUserModelId();
        }
        private void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);

        }

        private async Task CheckForUpdate()
        {
            try
            {
                using var updateManager = new GithubUpdateManager("https://github.com/Schtenk/Better-Windows-Mouse-Sensitivty");
                var result = await updateManager.UpdateApp();
                if(result != null)
                {
                    var msgBoxResult = MessageBox.Show("New update", "New Update Found", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.Yes) UpdateManager.RestartApp();
                }
            }
            catch
            {

            }
        }
    }
}