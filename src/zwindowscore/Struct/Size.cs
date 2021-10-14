using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace zwindowscore.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Size
    {
        public Int32 cx;
        public Int32 cy;

        public Size(Int32 cx, Int32 cy)
        { this.cx = cx; this.cy = cy; }
    }
}
