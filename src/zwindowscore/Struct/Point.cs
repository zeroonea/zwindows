using System.Runtime.InteropServices;

namespace zwindowscore.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        { 
            this.X = x;
            this.Y = y;
        }
    }
}
