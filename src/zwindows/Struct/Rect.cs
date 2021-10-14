
using System.Runtime.InteropServices;

namespace zwindows.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public override string ToString()
        {
            return $"{Left} {Top} {Right} {Bottom}";
        }
    }
}
