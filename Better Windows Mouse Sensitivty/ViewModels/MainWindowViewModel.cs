using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Better_Windows_Mouse_Sensitivty.MouseRegistryData;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Better_Windows_Mouse_Sensitivty.ViewModels
{
    public class MainWindowViewModel: ObservableObject
    {
        public MainWindowViewModel()
        {


            _confirmCommand = new RelayCommand(ConfirmSensitivity);
            _resetToLastBackupCommand = new RelayCommand(ResetToLastBackup);
            _resetToDefaultCommand = new RelayCommand(ResetToDefault);
        }

        private double _sensitivity;
        public double Sensitivity { get => _sensitivity; set => SetProperty(ref _sensitivity, Math.Round(value, 2)); }

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand { get => _confirmCommand; set => SetProperty(ref _confirmCommand, value); }

        private ICommand _resetToLastBackupCommand;
        public ICommand ResetToLastBackupCommand { get => _resetToLastBackupCommand; set => SetProperty(ref _resetToLastBackupCommand, value); }

        private ICommand _resetToDefaultCommand;
        public ICommand ResetToDefaultCommand { get => _resetToDefaultCommand; set => SetProperty(ref _resetToDefaultCommand, value); }


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
            var coordinate = BinaryPrimitives.ReadInt32LittleEndian(curve);
            if (isXCurve)
            {
                var flatCoordinate = BinaryPrimitives.ReadInt32LittleEndian(FlatWin10Values.SmoothMouseXCurve);
                return flatCoordinate / coordinate;
            }
            else
            {
                var flatCoordinate = BinaryPrimitives.ReadInt32LittleEndian(FlatWin10Values.SmoothMouseYCurve);
                return flatCoordinate / coordinate;
            }
        }

        public void ConfirmSensitivity()
        {
            var curve = ApplySensitivty(FlatWin10Values.SmoothMouseXCurve, 0.8);
            var test = BitConverter.ToString(curve);
        }

        public void ResetToLastBackup()
        {

        }

        public void ResetToDefault()
        {

        }
    }
}
