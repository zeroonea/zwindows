using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zwindowscore.Utils;
using zwindowscore.Struct;
using zwindowscore.Utils.UI;
using System.Drawing;
using static zwindowscore.Monitor;
using zwindowscore.Options;
using WindowsDesktop;

namespace zwindowscore
{
    public static partial class Global
    {
        public static Dictionary<string, DesktopAnchor> DesktopAnchors = new Dictionary<string, DesktopAnchor>();
        public static Dictionary<IntPtr, WindowTabButton> TabButtonWindows = new Dictionary<IntPtr, WindowTabButton>();
        public static Dictionary<string, ScreenTabButtonsBar> TabButtonsBars = new Dictionary<string, ScreenTabButtonsBar>();

        public static Dictionary<string, DesktopState> DesktopStates = new Dictionary<string, DesktopState>();

        public static List<WindowFilterRule> WindowFilterRules = new List<WindowFilterRule>();

        public static BindingList<ForegroundGroup> ForegroundGroups = new BindingList<ForegroundGroup>();
        public static ZWindowsSettings Settings { get; set; } = new ZWindowsSettings();

        public static BindingList<MonitorDeviceDefinition> MonitorDevices = new BindingList<MonitorDeviceDefinition>();
        public static BindingList<VirtualDesktopInfo> VirtualDesktops = new BindingList<VirtualDesktopInfo>();

        public static MonitorDevice CurrentMonitor;
        public static String CurrentDesktopName;
        public static bool IsDetectWindow = false;
        public static bool IsPauseEventChecking = false;
        public static ForegroundWindow CurrentForegroundWindow = new ForegroundWindow();
        public static TextDataSource CurrentForegroundWindowText = new TextDataSource();

        public static int TabButtonWidth = 100;

        public static Form Main;

        public static int GlobalMouseDragStartX;
        public static int GlobalMouseDragStartY;

        public static List<IntPtr> IgnoredMinimizeEvents = new List<IntPtr>();

        public static Dictionary<string, Cache<int>> HwndBorderSizes { get; set; } = new Dictionary<string, Cache<int>>();
        public static List<string> Colors = new List<string>
        {
            "#F44336",
            "#795548",
            "#2196F3",
            "#4CAF50",
            "#3F51B5",
            "#009688",
            "#E91E63",
            "#CDDC39",
            "#00BCD4",
            "#FFEB3B",
            "#8BC34A",
            "#673AB7",
            "#FF9800",
            "#9C27B0",
            
            "#607D8B",
            "#FF5722",
        };

        public static Color GetColor(string hex)
        {
            if (!CustomColors.ContainsKey(hex))
            {
                CustomColors[hex] = ColorTranslator.FromHtml(hex);
            }
            return CustomColors[hex];
        }

        public static Dictionary<string, Color> CustomColors = new Dictionary<string, Color>
        { 
            ["DragWindowTab"] = ColorTranslator.FromHtml("#ffab91")
        };

        public static Form GetDesktopAnchor(string vdn)
        {
            if (!DesktopAnchors.ContainsKey(vdn))
            {
                DesktopAnchors[vdn] = new DesktopAnchor();
            }
            return DesktopAnchors[vdn];
        }

        public static string SetCurrentDesktop(string dn = null)
        {
            if(dn == null)
            { 
                var cd = WindowsDesktop.VirtualDesktop.Current;
                if(cd != null)
                { 
                    Global.CurrentDesktopName = Win32Helper.GetDesktopName(cd.Id);
                }
            }
            else
            {
                Global.CurrentDesktopName = dn;
            }
            return Global.CurrentDesktopName;
        }

        public static string GetLayoutsFilePath()
        {
            var settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "layouts.json");
            return settingsFilePath;
        }

        public static Tuple<ZWindowsSettings, string> LoadLayoutsFromFile()
        {
            var json = string.Empty;
            ZWindowsSettings settings = null;
            var filePath = Global.GetLayoutsFilePath();
            if(File.Exists(filePath))
            {
                json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    settings = JsonConvert.DeserializeObject<ZWindowsSettings>(json);
                }
            }
            return Tuple.Create(settings, json);
        }

        public static string GetForegroundGroupsFilePath()
        {
            var settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "fgg.json");
            return settingsFilePath;
        }

        public static List<ForegroundGroup> LoadForegroundGroupsFromFile()
        {
            var filePath = Global.GetForegroundGroupsFilePath();
            if(File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<ForegroundGroup>>(json);
                }
            }
            return null;
        }

        public static string GetDesktopsStatesFilePath()
        {
            var settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ds.json");
            return settingsFilePath;
        }

        public static Dictionary<string, DesktopState> LoadDesktopsStatesFromFile()
        {
            var filePath = Global.GetDesktopsStatesFilePath();
            if(File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, DesktopState>>(json);
                }
            }
            return null;
        }

        public static void LoadVirtualDesktops()
        {
            VirtualDesktops.Clear();
            var desks = WindowsDesktop.VirtualDesktop.GetDesktops().ToList();
            desks.ForEach(p => VirtualDesktops.Add(new VirtualDesktopInfo{
                VirtualDesktop = p,
                Id = p.Id,
                Index = Win32Helper.GetDesktopIndex(p),
                Name = Win32Helper.GetDesktopName(p.Id, Win32Helper.GetDesktopIndex(p))
            }));
        }

        public static void LoadMonitors()
        {
            MonitorDevices.Clear();
            //Monitor.GetActiveMonitors();
            var monitors = Monitor.GetMonitors();
            if(monitors != null)
            {
                foreach(var mi in monitors)
                {
                    MonitorDevice md = null;
                    var mdi = GetMonitorDefinition(mi);
                    if(Settings != null && Settings.MonitorsLayouts != null
                        && Settings.MonitorsLayouts.ContainsKey(mdi.Id))
                    {
                        md = Settings.MonitorsLayouts[mdi.Id];
                    }
                    else
                    {
                        md = new MonitorDevice();
                        Settings.MonitorsLayouts[mdi.Id] = md;
                    }

                    mdi.Update(md);
                    ParseMonitor(mi, md);
                    MonitorDevices.Add(mdi);
                    Global.UpdateMonitorInfo(mi, md);
                }
            }
        }

        public static MonitorDevice UpdateMonitorInfo(MonitorDeviceInfo mi, MonitorDevice md = null)
        {
            if(md == null)
            {
                md = ParseMonitor(mi);
            }

            if (!Settings.MonitorsLayouts.ContainsKey(md.Id))
            {
                Settings.MonitorsLayouts.Add(md.Id, md);
            }
            else
            {
                md = Settings.MonitorsLayouts[md.Id];
                md.Id = md.Id;
                md.PortId = md.PortId;
                md.Name = md.Name;
                md.IsAvailable = true;
            }

            md.IsAvailable = true;            
            UpdateMonitorLayouts(md);
            return md;
        }

        public static string GetMonitorId(MonitorDeviceInfo mi)
        {
            var tmps = mi.DeviceId.ToLower().Split('\\').ToList();
            tmps.RemoveAt(tmps.Count - 1);
            return String.Join("_", tmps);
        }

        public static MonitorDeviceDefinition GetMonitorDefinition(MonitorDeviceInfo mi)
        {
            var md = new MonitorDeviceDefinition();
            var tmps = mi.DeviceId.ToLower().Split('\\').ToList();
            var name = tmps[1];
            var portId = tmps[tmps.Count - 1];
            tmps.RemoveAt(tmps.Count - 1);

            md.Id = mi.DeviceId.CleanMonitorId();
            md.Name = name;
            md.PortId = portId;

            return md;
        }


        public static string CleanMonitorId(this string monitorId)
        {
            var tmps = monitorId.ToLower().Split('\\').ToList();
            tmps.RemoveAt(tmps.Count - 1);
            return String.Join("_", tmps);
        }

        public static MonitorDevice ParseMonitor(MonitorDeviceInfo mi, MonitorDevice md = null)
        {
            if(md == null)
            {
                md = new MonitorDevice();
            }
            if(string.IsNullOrEmpty(md.Id))
            {
                GetMonitorDefinition(mi).Update(md);
            }

            md.RealWidth = mi.GetRealWidth();
            md.RealHeight = mi.GetRealHeight();
            md.Width = mi.GetWidth();
            md.Height = mi.GetHeight();
            md.X = mi.Monitor.Left;
            md.Y = mi.Monitor.Top;

            /*if(md.X > 0)
            {
                md.X = md.X + 1;
            }*/

            return md;
        }

        public static MonitorDevice UpdateMonitorLayouts(MonitorDevice monitor)
        {
            if(monitor == null)
            {
                monitor = new MonitorDevice();
            }
            if(monitor.Layouts != null && monitor.Layouts.Count > 0)
            {
                foreach(var layout in monitor.Layouts)
                {                 
                    UpdateLayout(monitor, layout);
                }
            }
            return monitor;
        }

        public static void UpdateLayout(MonitorDevice monitor, MonitorLayout layout)
        {
            var mw = Settings.HideTaskbars ? monitor.RealWidth : monitor.Width;
            var mh = Settings.HideTaskbars ? monitor.RealHeight : monitor.Height;

            layout.Left = (mw * layout.X) / 100;
            layout.Top = (mh * layout.Y) / 100;

            layout.Bottom = layout.Top + (mh * layout.Height) / 100;
            layout.Right = layout.Left + (mw * layout.Width) / 100;

            layout.CWidth = layout.Right - layout.Left;
            layout.CHeight = layout.Bottom - layout.Top;
                    
            if(layout.Left != 0)
            {
                layout.Left += 1;
                layout.CWidth -= 2;
            }
            if(layout.Top != 0)
            {
                layout.Top += 1;
                layout.CHeight -= 2;
            }
                    
            var dockX = layout.DockX != null ? layout.DockX.Value : layout.Left;
            var dockY = layout.DockY != null ? layout.DockY.Value : layout.Top + layout.CHeight / 2;

            layout.DockX = dockX;
            layout.DockY = dockY;

            layout._monitor = monitor;
        }

        public static int CountLayouts(MonitorDevice monitor, string desktopName = null)
        {
            if(desktopName == null)
            {
                desktopName = Global.CurrentDesktopName;
            }
            return monitor.Layouts.Count(p => p.DesktopName == desktopName || p.DesktopName == "default");
        }

        public static MonitorLayout GetMonitorLayout(Rect? windowPos, Struct.Point? mousePoint, string monitorId, string virtualDesktopName)
        {
            MonitorLayout nearestLayout = null;

            foreach(var mid in Settings.MonitorsLayouts.Keys)
            {
                if(monitorId != null && monitorId != mid) continue;

                var monitor = Settings.MonitorsLayouts[mid];
                int x = 0;
                int y = 0;

                if (mousePoint.HasValue)
                {
                    x = mousePoint.Value.X;
                    y = mousePoint.Value.Y;
                }
                else if(windowPos.HasValue)
                {
                    x = windowPos.Value.Left;
                    y = windowPos.Value.Top;
                }

                var nearestDistance = 0;

                foreach(var layout in monitor.Layouts)
                {
                    if(!string.IsNullOrEmpty(virtualDesktopName) 
                        && layout.DesktopName != virtualDesktopName 
                        && layout.DesktopName != "default") continue;
                    if(layout.MonitorRatio == 1 && !monitor.IsPortrait) continue;
                    if(layout.MonitorRatio == 2 && monitor.IsPortrait) continue;

                    var l = monitor.X + layout.Left;
                    var r = monitor.X + layout.Right;
                    var t = monitor.Y + layout.Top;
                    var b = monitor.Y + layout.Bottom;

                    if(
                        (x >= l || l == 0) 
                        && x <= r

                        && (y >= t || t == 0) 
                        && y <= b
                        )
                    {
                        var dis = (int)MyMath.GetDistance(x, y, 
                            layout.DockX.Value + monitor.X, 
                            layout.DockY.Value + monitor.Y);
                        if(dis < nearestDistance || nearestDistance == 0)
                        {
                            nearestDistance = dis;
                            nearestLayout = layout;
                        }
                    }
                }

                if(monitorId != null) break;
            }

            return nearestLayout;
        }

        public static List<DesktopWindow> GetDesktopWindows(bool currentVirtualDesktopOnly = false)
        {
            var collection = new List<DesktopWindow>();
            Win32Helper.EnumDelegate filter = delegate(IntPtr hWnd, int lParam)
            {
                if(IntPtr.Zero == hWnd) return true;

                // The window must be visible
                if (!Win32Helper.IsWindowVisible(hWnd))
                {
                    return true;
                }

                var result = new StringBuilder(255);
                Win32Helper.GetWindowText(hWnd, result, result.Capacity + 1);
                string title = result.ToString();

                if(string.IsNullOrEmpty(title) 
                    || Global.Settings.IgnoredProcessNames.Contains(title)) 
                    return true;

                Debug.WriteLine("----");
                Debug.WriteLine(title);

                if (!Win32Helper.IsAltTabWindow(hWnd))
                {
                    return true;
                }

                //if (currentVirtualDesktopOnly && 
                //    !IsHwndOnCurrentDesktop(hWnd))
                //{
                //    return true;
                //}

                //var isVisible = Win32Helper.IsWindowVisible(hWnd);
                var isIconic = Win32Helper.IsIconic(hWnd);

                //if(!isVisible && !isIconic)
                //{
                //    return true;
                //}

                collection.Add(new DesktopWindow { Handle = hWnd, Title = title, IsVisible = true, IsIconic = isIconic });

                return true;
            };

            Win32Helper.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero);
            return collection;
        }

        public static bool IsHwndOnCurrentDesktop(IntPtr hwnd)
        {
            try
            {
                return WindowsDesktop.VirtualDesktop.Current.Id == VirtualDesktop.FromHwnd(hwnd)?.Id;
            }
            catch
            {
                return false;
            }
        }

        public static WindowTabButton FindTabButtonWithHwnd(IntPtr hwnd)
        {
            return Global.TabButtonWindows.ContainsKey(hwnd)
                ? Global.TabButtonWindows[hwnd] : null;
        }
    }
}
