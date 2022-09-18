﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better_Windows_Mouse_Sensitivty.MouseRegistryData
{
    public class DefaultWin10Values
    {
        public const int MouseSensitivity = 10;
        public const byte MouseSpeed = 0;
        public static readonly new List<byte> SmoothMouseXCurve = new List<byte>
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x15, 0x6e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x40, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x29, 0xdc, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        public static readonly List<byte> SmoothMouseYCurve = new List<byte>
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xfd, 0x11, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x24, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0xfc, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0xc0, 0xbb, 0x01, 0x00, 0x00, 0x00, 0x00
        };
    }
}
