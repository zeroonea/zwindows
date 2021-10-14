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
using zwindows.Utils;
using zwindows.Struct;
using zwindows.Utils.UI;
using System.Drawing;

namespace zwindows
{
    public static class Global
    {
        //public static Form Parent;

        //public static Dictionary<string, List<ScreenTabButton>> TabButtons = new Dictionary<string, List<ScreenTabButton>>();

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
                Name = Win32Helper.GetDesktopName(p.Id)
            }));
        }

        public static void LoadMonitors()
        {
            MonitorDevices.Clear();
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

                    mdi.Update(md);
                    ParseMonitor(mi, md);
                    MonitorDevices.Add(mdi);
                    Global.UpdateMonitorInfo(mi, md);
                }
            }
        }

        public static MonitorDevice UpdateMonitorInfo(MonitorInfo mi, MonitorDevice md = null)
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

        public static string GetMonitorId(MonitorInfo mi)
        {
            var tmps = mi.DeviceName.ToLower().Split('\\').ToList();
            tmps.RemoveAt(tmps.Count - 1);
            return String.Join("_", tmps);
        }

        public static MonitorDeviceDefinition GetMonitorDefinition(MonitorInfo mi)
        {
            var md = new MonitorDeviceDefinition();
            var tmps = mi.DeviceName.ToLower().Split('\\').ToList();
            var name = tmps[1];
            var portId = tmps[tmps.Count - 1];
            tmps.RemoveAt(tmps.Count - 1);
            var id = String.Join("_", tmps);

            md.Id = id;
            md.Name = name;
            md.PortId = portId;

            return md;
        }

        public static MonitorDevice ParseMonitor(MonitorInfo mi, MonitorDevice md = null)
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
            layout.Left = (monitor.Width * layout.X) / 100;
            layout.Top = (monitor.Height * layout.Y) / 100;

            layout.Bottom = layout.Top + (monitor.Height * layout.Height) / 100;
            layout.Right = layout.Left + (monitor.Width * layout.Width) / 100;

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
            monitorId = monitorId.ToLower();
            if (Settings.MonitorsLayouts.ContainsKey(monitorId))
            {
                var monitor = Settings.MonitorsLayouts[monitorId];
                int x = 0;
                int y = 0;

                if (mousePoint.HasValue)
                {
                    x = mousePoint.Value.X - monitor.X;
                    y = mousePoint.Value.Y - monitor.Y;
                }
                else if(windowPos.HasValue)
                {
                    x = windowPos.Value.Left - monitor.X;
                    y = windowPos.Value.Top - monitor.Y;
                }

                /*var smallestArea = 0;*/
                var nearestDistance = 0;

                foreach(var layout in monitor.Layouts)
                {
                    if(!string.IsNullOrEmpty(virtualDesktopName) && layout.DesktopName != virtualDesktopName && layout.DesktopName != "default") continue;
                    if(layout.MonitorRatio == 1 && !monitor.IsPortrait) continue;
                    if(layout.MonitorRatio == 2 && monitor.IsPortrait) continue;

                    if(
                        (x >= layout.Left || layout.Left == 0) 
                        && x <= layout.Right

                        && (y >= layout.Top || layout.Top == 0) 
                        && y <= layout.Bottom
                        )
                    {
                        var dis = (int)MyMath.GetDistance(x, y, layout.DockX.Value, 
                            layout.DockY.Value);
                        if(dis < nearestDistance || nearestDistance == 0)
                        {
                            nearestDistance = dis;
                            nearestLayout = layout;
                        }
                        /*else if(dis == nearestDistance)
                        {
                            if(nearestLayout.Width * nearestLayout.Height >= layout.Width * layout.Height)
                            {
                                nearestLayout = layout;
                            }
                        }*/
                    }
                }
            }

            return nearestLayout;
        }

        public static List<DesktopWindow> GetDesktopWindows(bool currentVirtualDesktopOnly = false)
        {
            var collection = new List<DesktopWindow>();
            Win32Helper.EnumDelegate filter = delegate(IntPtr hWnd, int lParam)
            {
                if (currentVirtualDesktopOnly && 
                    !IsHwndOnCurrentDesktop(hWnd))
                {
                    return true;
                }
                var result = new StringBuilder(255);
                Win32Helper.GetWindowText(hWnd, result, result.Capacity + 1);
                string title = result.ToString();

                if(string.IsNullOrEmpty(title) 
                    || Global.Settings.IgnoredProcessNames.Contains(title)) 
                    return true;

                var isVisible = Win32Helper.IsWindowVisible(hWnd);
                var isIconic = Win32Helper.IsIconic(hWnd);

                if(!isVisible && !isIconic)
                {
                    return true;
                }

                collection.Add(new DesktopWindow { Handle = hWnd, Title = title, IsVisible = isVisible, IsIconic = isIconic });

                return true;
            };

            Win32Helper.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero);
            return collection;
        }

        public static bool IsHwndOnCurrentDesktop(IntPtr hwnd)
        {
            return WindowsDesktop.VirtualDesktop.Current.Id == WindowsDesktop.VirtualDesktop.FromHwnd(hwnd).Id;
        }
    }

    public class DesktopWindow
    {
        public IntPtr Handle { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; }
        public bool IsIconic { get; set; }
    }


    public class ForegroundGroup
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string Title
        {
            get
            {
                return $"{Name} ({Windows.Count})";
            }
        }
        public BindingList<ForegroundWindow> Windows { get; set; } = new BindingList<ForegroundWindow>();
    }

    public class ForegroundWindow
    {
        public string Name { get; set; }
        public IntPtr Hwnd { get; set; }
    }

    public class ZWindowsSettings
    { 
        public string TabBarColor = "#333";
        public string ActiveWindowTabButtonColor = "#333";
        public string ActiveWindowTabButtonTextColor = "#ff6b81";
        public bool DisabledMinimize { get; set; } = true;
        public bool DefaultWindowsTabNotificationOn { get; set; } = true;
        public int DeltaForAutoDetectWindowLayout { get ;set; } = 100;
        public List<string> IgnoredProcessNames = new List<string>();
        public Dictionary<string, MonitorDevice> MonitorsLayouts { get; set; } = new Dictionary<string, MonitorDevice>();
        public List<WindowEventListener> WindowEventListeners { get; set; } = new List<WindowEventListener>();
    }

    public class MonitorDeviceDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PortId { get; set; }

        public void Update(MonitorDevice md)
        {
            md.Id = Id;
            md.Name = Name;
            md.PortId = PortId;
        }
    }

    public class MonitorDevice
    {
        public bool IsAvailable { get; set; } = false;

        public string Id { get; set; }
        public string Name { get; set; }
        public string PortId { get; set; }
        public int RealWidth { get; set; }
        public int RealHeight { get; set; }

        [JsonIgnore]
        public bool IsPortrait
        {
            get
            {
                return RealWidth < RealHeight;
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public List<MonitorLayout> Layouts { get; set; } = new List<MonitorLayout>();
    }

    public class MonitorLayout
    {
        [JsonIgnore]
        public string Title
        {
            get
            {
                return $"X: {X}% Y: {Y}% Width: {Width}% Height: {Height}%";
            }
        }

        [JsonIgnore]
        public ScreenBoundingRectangle _rectangle { get; set; }

        public string DesktopName { get; set; } = "default";

        public MonitorLayoutTabsBar TabsBar { get; set; } = new MonitorLayoutTabsBar();

        public int? DockX { get; set; } = null;
        public int? DockY { get; set; } = null;

        // Percent of Monitor
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Pixel (auto calculated)
        [JsonIgnore]
        public int Right { get; set; }
        [JsonIgnore]
        public int Bottom { get; set; }
        [JsonIgnore]
        public int Left { get; set; } // X
        [JsonIgnore]
        public int Top { get; set; } // Y

        [JsonIgnore]
        public int CWidth { get; set; }
        [JsonIgnore]
        public int CHeight { get; set; }

        [JsonIgnore]
        public int LeftOffset
        {
            get
            {
                return Offset[3];
            }
            set
            {
                Offset[3] = value;
            }
        }

        [JsonIgnore]
        public int TopOffset
        {
            get
            {
                return Offset[0];
            }
            set
            {
                Offset[0] = value;
            }
        }

        [JsonIgnore]
        public int RightOffset
        {
            get
            {
                return Offset[1];
            }
            set
            {
                Offset[1] = value;
            }
        }

        [JsonIgnore]
        public int BottomOffset
        {
            get
            {
                return Offset[2];
            }
            set
            {
                Offset[2] = value;
            }
        }

        public int[] Offset { get; set; } = new int[4] {0, 0, 0, 0}; // Top, Right, Bottom, Left
        public int MonitorRatio { get; set; } = 0; // 0: for all, 1: for portrait only, 2: for landscape only
    }

    public class MonitorLayoutTabsBar
    {
        public bool Enable { get; set; } = true;
        public int MaxWidth { get; set; } = 0;
        //public int Align { get; set; } = 0; // 0: center, 1: left, 2: right
        public int LeftOffset { get; set; } = 0;
        public double Opacity { get; set; } = 0.7;
    }

    public class Cache<T>
    {
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public T Value { get; set; }
    }

    public class WindowEventListener
    {
        public string Type { get; set; } // title
        public WindowFilterRule Rule { get; set; }

        [JsonIgnore]
        public List<IntPtr> Hwnds { get; set; }
    }

    public class WindowFilterRule
    {
        public bool MatchMultiple { get; set; }
        public string RegexTitle { get; set; }
        public string ExactTitle { get; set; }
        public string ExactProcessName { get; set; }
        public string ExactExeFileName { get; set; }
    }

    public class DesktopState
    {
        public string DesktopName { get; set; }
        public List<WindowState> WindowStates { get; set; }
    }

    public class WindowState
    {
        public IntPtr Hwnd { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class VirtualDesktopInfo
    {
        [JsonIgnore]
        public WindowsDesktop.VirtualDesktop VirtualDesktop { get; set; }

        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
