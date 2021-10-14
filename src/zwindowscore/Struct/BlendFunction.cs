using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace zwindowscore.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlendFunction
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }
}
