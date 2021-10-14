using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace zwindowscore.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGB
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }
}
