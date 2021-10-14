using System.Runtime.InteropServices;

namespace zwindows.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;
    }
}
