using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using Better_Windows_Mouse_Sensitivty.Localization;
using Better_Windows_Mouse_Sensitivty.Models;
using Microsoft.Win32;

namespace Better_Windows_Mouse_Sensitivty.MouseRegistryData
{
    public class RegistryHelper
    {
        public const string BackupPath = "backups\\";
        public const string BackupFilePaththoutExt = $"{BackupPath}backup";
        public const string BackupJsonExt = ".json";
        public const string BackupRegExt = ".reg";

        const string BasePath = "Control Panel\\Mouse";
        const string MouseSensitivity = "MouseSensitivity";
        const string MouseSpeed = "MouseSpeed";
        const string SmoothMouseXCurve = "SmoothMouseXCurve";
        const string SmoothMouseYCurve = "SmoothMouseYCurve";

        private static object GetRegistryValue(string valueKey)
        {
            var key = Registry.CurrentUser.OpenSubKey(BasePath);
            if (key == null) { throw new NullReferenceException($"{Application.Current.Resources[Keys.OpenRegPathFailedMessage]} {BasePath}"); }
            var value = key.GetValue(valueKey);
            if (value == null) { throw new NullReferenceException($"{Application.Current.Resources[Keys.GetRegValueFailedMessage]} {valueKey}."); }
            return value;
        }

        private static void SetRegistryValue(string valueKey, object data, RegistryValueKind valueKind)
        {
            var key = Registry.CurrentUser.OpenSubKey(BasePath, true);
            if (key == null) { throw new NullReferenceException($"{Application.Current.Resources[Keys.OpenRegPathFailedMessage]} {BasePath}"); }
            key.SetValue(valueKey, data, valueKind);
        }

        #region GetFunctions
        public static MouseRegistryModel GetMouseRegistryModel()
        {
            return new MouseRegistryModel()
            {
                MouseSensitivity = GetMouseSensitivity(),
                MouseSpeed = GetMouseSpeed(),
                SmoothMouseXCurve = GetSmoothMouseXCurve(),
                SmoothMouseYCurve = GetSmoothMouseYCurve()
            };
        }

        public static int GetMouseSensitivity()
        {
            return int.Parse((string)GetRegistryValue(MouseSensitivity));
        }

        public static byte GetMouseSpeed()
        {
            return byte.Parse((string)GetRegistryValue(MouseSpeed));
        }

        public static byte[] GetSmoothMouseXCurve()
        {
            return (byte[])GetRegistryValue(SmoothMouseXCurve);
        }

        public static byte[] GetSmoothMouseYCurve()
        {
            return (byte[])GetRegistryValue(SmoothMouseYCurve);
        }
        #endregion

        #region SetFunctions
        public static void SetFromMouseRegistryModel(MouseRegistryModel model)
        {
            SetMouseSensitivity(model.MouseSensitivity);
            SetMouseSpeed(model.MouseSpeed);
            SetSmoothMouseXCurve(model.SmoothMouseXCurve);
            SetSmoothMouseYCurve(model.SmoothMouseYCurve);
        }

        public static void SetMouseSensitivity(int mouseSensitivity)
        {
            SetRegistryValue(MouseSensitivity, mouseSensitivity, RegistryValueKind.String);
        }

        public static void SetMouseSpeed(byte mouseSpeed)
        {
            SetRegistryValue(MouseSpeed, mouseSpeed, RegistryValueKind.String);
        }
        
        public static void SetSmoothMouseXCurve(byte[] curve)
        {
            SetRegistryValue(SmoothMouseXCurve, curve, RegistryValueKind.Binary);
        }

        public static void SetSmoothMouseYCurve(byte[] curve)
        {
            SetRegistryValue(SmoothMouseYCurve, curve, RegistryValueKind.Binary);
        }
        #endregion

        public static void CreateBackup()
        {
            var model = GetMouseRegistryModel();

            var json = JsonSerializer.Serialize(model, options: new JsonSerializerOptions
            {
                WriteIndented = true
            });
            Directory.CreateDirectory(BackupPath);
            File.WriteAllText($"{BackupFilePaththoutExt}{BackupJsonExt}", json);

            using var writer = File.CreateText($"{BackupFilePaththoutExt}{BackupJsonExt}-{DateTime.Now.ToString("yyyy_MM_dd(HH_mm_ss)")}{BackupRegExt}");
            writer.WriteLine("Windows Registry Editor Version 5.00");
            writer.WriteLine("");
            writer.WriteLine($"[{Registry.CurrentUser.Name}\\{BasePath}]");
            writer.WriteLine($"\"{MouseSensitivity}\"=\"{model.MouseSensitivity}\"");
            writer.WriteLine($"\"{MouseSpeed}\"=\"{model.MouseSpeed}\"");

            var line = $"hex:";
            for(int i = 0; i < model.SmoothMouseXCurve.Length; i += 8)
            {
                line += string.Join(",", model.SmoothMouseXCurve.Skip(i).Take(8).Select(x => x.ToString("X2")));
                if (i < model.SmoothMouseXCurve.Length - 8) line += $",\\{Environment.NewLine}\t";
            }
            writer.WriteLine($"\"{SmoothMouseXCurve}\"={line}");

            line = "hex:";
            for(int i = 0; i < model.SmoothMouseYCurve.Length; i += 8)
            {
                line += string.Join(",", model.SmoothMouseYCurve.Skip(i).Take(8).Select(x => x.ToString("X2")));
                if (i < model.SmoothMouseYCurve.Length - 8) line += $",\\{Environment.NewLine}\t";
            }
            writer.WriteLine($"\"{SmoothMouseYCurve}\"={line}");
        }
    }
}
