using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace zwindows.Struct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MonitorInfo
    {
        public int Size;
        public Rect Monitor;
        public Rect WorkArea;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        public int GetRealWidth()
        {
            return Monitor.Right - Monitor.Left;
        }

        public int GetRealHeight()
        {
            return Monitor.Bottom - Monitor.Top;
        }

        public int GetWidth()
        {
            return WorkArea.Right - WorkArea.Left;
        }

        public int GetHeight()
        {
            return WorkArea.Bottom - WorkArea.Top;
        }
    }
}
