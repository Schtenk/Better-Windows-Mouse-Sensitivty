using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better_Windows_Mouse_Sensitivty.MouseRegistryData
{
    public class FlatWin10Values
    {
        public const int MouseSensitivity = 10;
        public const byte MouseSpeed = 1;

        public static readonly byte[] SmoothMouseXCurve = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xC0, 0xCC, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x80, 0x99, 0x19, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x66, 0x26, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x33, 0x33, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        public static readonly byte[] SmoothMouseYCurve = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x38, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xA8, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xE0, 0x00, 0x00, 0x00, 0x00, 0x00
        };
    }
}
