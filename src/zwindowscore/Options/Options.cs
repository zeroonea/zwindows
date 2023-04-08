using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using zwindowscore.Utils;

namespace zwindowscore.Options
{
    public class ZWindowsSettings
    { 
        public string TabBarColor = "#333";
        public string ActiveWindowTabButtonColor = "#333";
        public string ActiveWindowTabButtonTextColor = "#ff6b81";
        public string WindowDragOverlayColor = "#2192FF";
        
        public bool DisableDragWindowOverlay { get; set; } = false;
        public bool DisableAutoSnapWindowAfterDrag { get; set; } = false;

        public bool DisabledMinimize { get; set; } = true;
        public bool DefaultWindowsTabNotificationOn { get; set; } = true;
        public int DeltaForAutoDetectWindowLayout { get ;set; } = 200;

        public int CenterTaskBarDelay { get ;set; } = 5000;

        public float TabButtonsOpacity { get; set; } = 1f;

        public int TabButtonIconSize { get; set; } = 20;
        public int TabButtonWidth { get; set; } = 100;
        public int TabButtonHeight { get; set; } = 20;

        public bool LockTabButtonBetweenDesktops { get; set; } = false;
        public bool NotifyDesktopName { get; set; } = false;

        public bool HideTaskbars { get; set; } = true;

        public List<string> IgnoredProcessNames = new List<string>()
        {
            //"Windows Input Experience",
            //"Hidden Window",
            //"Loading...",
            //"Setup",
            //"Program Manager",

            //"XWin Msg Window",
            //"xwinclip",
            
            //"NVIDIA GeForce Overlay DT",
            //"NVIDIA GeForce Overlay"
        };

        public List<string> NoTabBarDesktopNames = new List<string>();

        public Dictionary<string, MonitorDevice> MonitorsLayouts { get; set; } = new Dictionary<string, MonitorDevice>();
        public List<WindowEventListener> WindowEventListeners { get; set; } = new List<WindowEventListener>();

        public List<string> NoFullSnapWhenAutoSnapProcesses = new List<string>()
        {
            //"notepad"
        };

        public int MultiTabsBugThreadSleep { get; set; } = 100;
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

        // Useable size (include or exclude task bar size)
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

        [JsonIgnore]
        public MonitorDevice _monitor { get; set; }

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

        public int[] Offset { get; set; } = new int[4] {20, 20, 20, 20}; // Top, Right, Bottom, Left
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
        public int Index { get; set; }
        public string Name { get; set; }
    }
}
