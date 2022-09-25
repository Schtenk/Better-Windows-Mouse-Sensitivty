using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Better_Windows_Mouse_Sensitivty.Helper;
using Better_Windows_Mouse_Sensitivty.Localization;
using Better_Windows_Mouse_Sensitivty.ViewModels;
using Better_Windows_Mouse_Sensitivty.Views;
using ControlzEx.Theming;
using Microsoft.Win32;
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

        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const string RegistryValueName = "AppsUseLightTheme";

        public enum WindowsTheme
        {
            Light,
            Dark
        }

        public void WatchForThemeChange()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            string query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                currentUser.User?.Value,
                RegistryKeyPath.Replace(@"\", @"\\"),
                RegistryValueName);

            try
            {
                var watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += (sender, args) =>
                {
                    SetThemeBasedOnWindowsTheme();
                };

                watcher.Start();
            }
            catch (Exception)
            {
            }

            WindowsTheme initialTheme = GetWindowsTheme();
        }

        private static WindowsTheme GetWindowsTheme()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object? registryValueObject = key?.GetValue(RegistryValueName);
                if (key != null && registryValueObject != null)
                {
                    int registryValue = (int)registryValueObject;

                    return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
                }
                else
                {
                    return WindowsTheme.Dark;
                }
            }
        }

        public static void SetThemeBasedOnWindowsTheme()
        {
            if (GetWindowsTheme() == WindowsTheme.Dark)
            {
                ThemeManager.Current.ChangeTheme(Current, "Dark.Blue");
            }
            else
            {
                ThemeManager.Current.ChangeTheme(Current, "Light.Blue");
            }
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
                Current.Resources.MergedDictionaries[0] = dict;
            }

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
                using var updateManager = new GithubUpdateManager("http://github.com/Schtenk/Better-Windows-Mouse-Sensitivty");
                var result = await updateManager.UpdateApp();
                if(result != null)
                {
                    var popupResult = PopupDialog.ShowDialog(
                        $"{Current.Resources[Keys.UpdateInstalledTitle]}",
                        $"{Current.Resources[Keys.UpdateInstalledMessage]}",
                        PopupButtons.YesNo);

                    if (popupResult == PopupResult.Yes) UpdateManager.RestartApp();
                }
            }
            catch
            {

            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: OnAppInstall,
                onAppUninstall: OnAppUninstall,
                onEveryRun: OnAppRun);

            WatchForThemeChange();
            SetThemeBasedOnWindowsTheme();

            SetLocalizationDictionary(Thread.CurrentThread.CurrentUICulture.ToString());

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

            CheckForUpdate();

        }
    }
}
