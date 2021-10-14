using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using zwindows.Struct;

namespace zwindows
{
    public static class Monitor
    {
        public const Int32 MONITOR_DEFAULTTOPRIMERTY = 0x00000001;
        public const Int32 MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport( "user32.dll" )]
        public static extern IntPtr MonitorFromWindow( IntPtr handle, Int32 flags );

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpmi, uint dwFlags);


        private static List<MonitorInfo> _monitors;

        static bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
        {
            MonitorInfo mi = new MonitorInfo();
            mi.Size = Marshal.SizeOf(typeof(MonitorInfo));
            if (GetMonitorInfo(hMonitor, ref mi))
            {
                Console.WriteLine(mi.DeviceName);
                if (!string.IsNullOrEmpty(mi.DeviceName))
                {
                    DisplayDevice dd = new DisplayDevice();
                    dd.cb = Marshal.SizeOf(dd);
                    EnumDisplayDevices(mi.DeviceName, 0, ref dd, 0);
                    mi.DeviceName = dd.DeviceID;
                }
                _monitors.Add(mi);
                Console.WriteLine(mi.DeviceName);
            }
            return true;
        }

        public static List<MonitorInfo> GetMonitors()
        {
            _monitors = new List<MonitorInfo>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumProc, IntPtr.Zero);
            return _monitors;
        }

        public static MonitorInfo? GetMonitorFromWindow(IntPtr handle, Int32 flags = MONITOR_DEFAULTTONEAREST)
        {
            var hMonitor = MonitorFromWindow(handle, flags);
            if(hMonitor != IntPtr.Zero)
            {
                MonitorInfo mi = new MonitorInfo();
                mi.Size = Marshal.SizeOf(typeof(MonitorInfo));
                if (GetMonitorInfo(hMonitor, ref mi))
                {
                    if (!string.IsNullOrEmpty(mi.DeviceName))
                    {
                        DisplayDevice dd = new DisplayDevice();
                        dd.cb = Marshal.SizeOf(dd);
                        EnumDisplayDevices(mi.DeviceName, 0, ref dd, 0);
                        mi.DeviceName = dd.DeviceID;
                        /*Console.WriteLine(dd.DeviceName);
                        Console.WriteLine(dd.DeviceID);
                        Console.WriteLine(dd.DeviceKey);*/
                    }
                    return mi;
                }
            }
            return null;
        }
    }
}
