using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using zwindowscore.Enum;
using zwindowscore.Struct;

namespace zwindowscore
{
    public static class Monitor
    {
        public const Int32 MONITOR_DEFAULTTOPRIMERTY = 0x00000001;
        public const Int32 MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport( "user32.dll" )]
        public static extern IntPtr MonitorFromWindow( IntPtr handle, Int32 flags );

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpmi, uint dwFlags);

        delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        public static void GetActiveMonitors()
        {
            DisplayDevice d = new DisplayDevice();
            d.cb = Marshal.SizeOf(d);
            try
            {
                for (uint id = 0; EnumDisplayDevices(null, id, ref d, 0); id++)
                {
                    if (d.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                    {
                        Console.WriteLine(
                            String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                                     id,
                                     d.DeviceName,
                                     d.DeviceString,
                                     d.StateFlags,
                                     d.DeviceID,
                                     d.DeviceKey
                                     )
                                     );
                        d.cb = Marshal.SizeOf(d);
                        EnumDisplayDevices(d.DeviceName, 0, ref d, 0);
                        Console.WriteLine(
                            String.Format("{0}, {1}",
                                     d.DeviceName,
                                     d.DeviceString
                                     )
                                     );

                        /*MonitorInfo mi = new MonitorInfo();
                        mi.Size = Marshal.SizeOf(typeof(MonitorInfo));
                        if (GetMonitorInfo(hMonitor, ref mi))*/
                    }
                    d.cb = Marshal.SizeOf(d);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0}", ex.ToString()));
            }
        }


        private static List<MonitorDeviceInfo> _monitors;

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
                    uint i = 0;
                    while(i <= 10)
                    {
                        EnumDisplayDevices(mi.DeviceName, i, ref dd, 0);
                        if (dd.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                        {
                            _monitors.Add(new MonitorDeviceInfo{
                                Size = mi.Size,
                                Monitor = mi.Monitor,
                                WorkArea = mi.WorkArea,
                                Flags = mi.Flags,
                                DeviceName = mi.DeviceName,
                                DeviceId = dd.DeviceID
                            });
                        }
                        i++;
                    }
                }
            }
            return true;
        }

        public static List<MonitorDeviceInfo> GetMonitors()
        {
            _monitors = new List<MonitorDeviceInfo>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumProc, IntPtr.Zero);
            return _monitors;
        }

        public static MonitorDeviceInfo GetMonitorFromWindow(IntPtr handle, Int32 flags = MONITOR_DEFAULTTONEAREST)
        {
            var hMonitor = MonitorFromWindow(handle, flags);
            if(hMonitor != IntPtr.Zero)
            {
                MonitorInfo mi = new MonitorInfo();
                mi.Size = Marshal.SizeOf(typeof(MonitorInfo));
                if (GetMonitorInfo(hMonitor, ref mi))
                {
                    var mdi = new MonitorDeviceInfo{
                        Size = mi.Size,
                        Monitor = mi.Monitor,
                        WorkArea = mi.WorkArea,
                        Flags = mi.Flags,
                        DeviceName = mi.DeviceName
                    };

                    if (!string.IsNullOrEmpty(mi.DeviceName))
                    {
                        DisplayDevice dd = new DisplayDevice();
                        dd.cb = Marshal.SizeOf(dd);

                        uint i = 0;
                        while(i <= 10)
                        {
                            EnumDisplayDevices(mi.DeviceName, i, ref dd, 0);
                            if (dd.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                            {
                                mdi.DeviceId = dd.DeviceID;
                                break;
                            }
                            i++;
                        }
                    }
                    return mdi;
                }
            }
            return null;
        }

        

        public class MonitorDeviceInfo
        {
            public int Size;
            public Rect Monitor;
            public Rect WorkArea;
            public uint Flags;
            public string DeviceName;
            public string DeviceId;

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
}
