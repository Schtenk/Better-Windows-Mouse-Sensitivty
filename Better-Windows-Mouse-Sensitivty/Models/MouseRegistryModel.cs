using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better_Windows_Mouse_Sensitivty.Models
{
    public class MouseRegistryModel
    {
        public int MouseSensitivity { get; set; }
        public byte MouseSpeed { get; set; }
        public byte[] SmoothMouseXCurve { get; set; }
        public byte[] SmoothMouseYCurve { get; set; }
    }
}
