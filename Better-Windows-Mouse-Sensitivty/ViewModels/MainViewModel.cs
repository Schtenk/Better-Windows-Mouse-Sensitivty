using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Better_Windows_Mouse_Sensitivty.Helper;
using Better_Windows_Mouse_Sensitivty.Localization;
using Better_Windows_Mouse_Sensitivty.Models;
using Better_Windows_Mouse_Sensitivty.MouseRegistryData;
using Better_Windows_Mouse_Sensitivty.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace Better_Windows_Mouse_Sensitivty.ViewModels
{
    public class MainViewModel: ObservableObject
    {

        public MainViewModel()
        {
            _confirmCommand = new RelayCommand(ConfirmNewSensitivity);
            _restoreLastBackupCommand = new RelayCommand(RestoreLastBackup);
            _resetToDefaultCommand = new RelayCommand(ResetToDefault);

            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            version = version == null ? "" : $"({version})";
            _windowTitle = $"{Application.Current.Resources[Keys.WindowTitle]} {version}";

            var curve = RegistryHelper.GetSmoothMouseXCurve();
            if (Enumerable.SequenceEqual(curve, DefaultWin10Values.SmoothMouseXCurve))
            {
                _sensitivity = 1.0d;
            }
            else
            {
                _sensitivity = GetSensitivity(curve);
            }

            SensitivityMin = 0.01d;
            SensitivityMax = 3.0d;
            SensitivitySmallChange = 0.01d;
            SensitivityLargeChange = 0.1d;
        }

        private string _windowTitle;
        public string WindowTitle { get => _windowTitle; set => SetProperty(ref _windowTitle, value); }


        private double _sensitivity;
        public double Sensitivity { get => _sensitivity; set => SetProperty(ref _sensitivity, Math.Round(value, 2)); }

        private double _sensitivityMin;
        public double SensitivityMin { get => _sensitivityMin; set => SetProperty(ref _sensitivityMin, value); }

        private double _sensitivityMax;
        public double SensitivityMax { get => _sensitivityMax; set => SetProperty(ref _sensitivityMax, value); }

        private double _sensitivitySmallChange;
        public double SensitivitySmallChange { get => _sensitivitySmallChange; set => SetProperty(ref _sensitivitySmallChange, value); }

        private double _sentivitityLargeChange;
        public double SensitivityLargeChange { get => _sentivitityLargeChange; set => SetProperty(ref _sentivitityLargeChange, value); }

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand { get => _confirmCommand; set => SetProperty(ref _confirmCommand, value); }

        private ICommand _restoreLastBackupCommand;
        public ICommand RestoreLastBackupCommand { get => _restoreLastBackupCommand; set => SetProperty(ref _restoreLastBackupCommand, value); }

        private ICommand _resetToDefaultCommand;
        public ICommand ResetToDefaultCommand { get => _resetToDefaultCommand; set => SetProperty(ref _resetToDefaultCommand, value); }

        public void ConfirmNewSensitivity()
        {
            RegistryHelper.CreateBackup();
            var curve = ApplySensitivty(FlatWin10Values.SmoothMouseXCurve, Sensitivity);
            RegistryHelper.SetMouseSensitivity(FlatWin10Values.MouseSensitivity);
            RegistryHelper.SetMouseSpeed(FlatWin10Values.MouseSpeed);
            RegistryHelper.SetSmoothMouseXCurve(curve);
            RegistryHelper.SetSmoothMouseYCurve(FlatWin10Values.SmoothMouseYCurve);

            PopupDialog.ShowDialog($"{Application.Current.Resources[Keys.ConfirmConfirmationMessage]}",
                $"{Application.Current.Resources[Keys.ConfirmConfirmationTitle]}");
        }

        public void RestoreLastBackup()
        {
            var model = JsonSerializer.Deserialize<MouseRegistryModel>(File.ReadAllText($"{RegistryHelper.BackupFilePaththoutExt}{RegistryHelper.BackupJsonExt}"));
            if (model == null)
            {
                PopupDialog.ShowDialog($"{Application.Current.Resources[Keys.BackupLoadFailedMessage]}",
                    $"{Application.Current.Resources[Keys.BackupLoadFailedTitle]}");
                return;
            }
            RegistryHelper.SetFromMouseRegistryModel(model);
            Sensitivity = GetSensitivity(RegistryHelper.GetSmoothMouseXCurve());
            PopupDialog.ShowDialog($"{Application.Current.Resources[Keys.RestoreLastBackupConfirmationMessage]}",
                $"{Application.Current.Resources[Keys.RestoreLastBackupConfirmationTitle]}");
        }

        public void ResetToDefault()
        {
            RegistryHelper.SetMouseSensitivity(DefaultWin10Values.MouseSensitivity);
            RegistryHelper.SetMouseSpeed(DefaultWin10Values.MouseSpeed);
            RegistryHelper.SetSmoothMouseXCurve(DefaultWin10Values.SmoothMouseXCurve);
            RegistryHelper.SetSmoothMouseYCurve(DefaultWin10Values.SmoothMouseYCurve);
            Sensitivity = 1.0d;
            PopupDialog.ShowDialog($"{Application.Current.Resources[Keys.ResetToDefaultConfirmationMessage]}",
                $"{Application.Current.Resources[Keys.ResetToDefaultConfirmationTitle]}");
        }

        public byte[] ApplySensitivty(ReadOnlySpan<byte> curve, double sensitivity, bool isXCurve = true)
        {
            var result = curve.ToArray();
            for (int i = 0; i < curve.Length; i += 8)
            {
                var coordinate = BinaryPrimitives.ReadInt32LittleEndian(result.Skip(i).Take(4).ToArray());
                if (isXCurve)
                {
                    coordinate = (int)Math.Round(coordinate / sensitivity);
                }
                else
                {
                    coordinate = (int)Math.Round(coordinate * sensitivity);
                }
                var newCoordinate = new byte[4];
                BinaryPrimitives.WriteInt32LittleEndian(newCoordinate, coordinate);
                Array.Copy(newCoordinate, 0, result, i, 4);
            }
            return result;
        }

        public double GetSensitivity(ReadOnlySpan<byte> curve, bool isXCurve = true)
        {
            var arr = curve.ToArray();
            int coordinate = 0;
            double sensitivity = 0.0d;
            double c = 0.0d;
            for (int i = 0; i < arr.Length; i += 8)
            {
                coordinate = BinaryPrimitives.ReadInt32LittleEndian(arr.Skip(i).Take(4).ToArray());
                if(coordinate != 0)
                {
                    if (isXCurve)
                    {
                        var flatCoordinate = BinaryPrimitives.ReadInt32LittleEndian(FlatWin10Values.SmoothMouseXCurve.Skip(i).Take(4).ToArray());
                        sensitivity += (double)flatCoordinate / coordinate;
                    }
                    else
                    {
                        var flatCoordinate = BinaryPrimitives.ReadInt32LittleEndian(FlatWin10Values.SmoothMouseYCurve.Skip(i).Take(4).ToArray());
                        sensitivity +=  (double)coordinate / flatCoordinate;
                    }
                    c++;
                }
            }
            sensitivity /= c;

            return Math.Round(sensitivity, 2);
        }
    }
}
