using System;
using System.Runtime.InteropServices;
using zwindows.Enum;
using zwindows.Struct;
using System.Linq;
using System.Text;
using zwindows.Utils;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;

namespace zwindows
{
    public static class Win32Helper
    {
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        delegate void WinEventDelegate(IntPtr hWinEventHook, EventType eventType,
            IntPtr hwnd, WinEventObjectId idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        const uint WINEVENT_OUTOFCONTEXT = 0;
        const uint WINEVENT_INCONTEXT = 4;
        const uint WINEVENT_SKIPOWNPROCESS = 2;

        static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);
        static IntPtr foreground;
        static MonitorInfo? currentMonitor;
        static Guid? currentDesktopId;

        public static bool PauseWinEventHook = false;
        //static List<IntPtr> ignoredNextFgEvent = new List<IntPtr>();
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;

        public static void StartEventHook()
        {
            IntPtr hhook = SetWinEventHook(EventType.EVENT_MIN, 
                    EventType.EVENT_MAX, 
                    IntPtr.Zero,
                    procDelegate, 0, 0, 
                    WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
        }

        public static string GetDesktopName(Guid? desktopId)
        {
            if (desktopId == null) return null;

            string desktopName = null;
            try
            {
                desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktops\\Desktops\\{" + desktopId.ToString() + "}", "Name", null);
            }
            catch { }
            return desktopName;
        }

        public static string GetDesktop(IntPtr hwnd)
        {
            var vd = WindowsDesktop.VirtualDesktop.FromHwnd(hwnd);
            if(vd != null)
            {
                return GetDesktopName(vd.Id);
            }

            return null;
        }

        public static void SetDesktop(IntPtr hwnd)
        {
            var vd = WindowsDesktop.VirtualDesktop.FromHwnd(hwnd);
            if(vd != null)
            {
                if(!currentDesktopId.HasValue || vd.Id != currentDesktopId.Value)
                {
                    currentDesktopId = vd.Id;
                    Console.WriteLine("Desktop {0} #{1}", Global.CurrentDesktopName, currentDesktopId);
                    Global.CurrentDesktopName = GetDesktopName(vd.Id);
                }
            }
            else
            {
                Console.WriteLine("Cannot get current desktop id");
            }
        }

        public static void SetDesktopAndMonitor(IntPtr hwnd)
        {
            SetDesktop(hwnd);

            var monitorInfo = Monitor.GetMonitorFromWindow(hwnd, Monitor.MONITOR_DEFAULTTONEAREST);
            if(monitorInfo != null)
            {
                if(currentMonitor == null || currentMonitor.Value.DeviceName != monitorInfo.Value.DeviceName)
                {
                    Console.WriteLine("Monitor {0}: {1}x{2}, {3}x{4}", 
                        monitorInfo?.DeviceName, 
                        monitorInfo?.GetRealWidth(), 
                        monitorInfo?.GetRealHeight(), 
                        monitorInfo?.GetWidth(),
                        monitorInfo?.GetHeight());

                    currentMonitor = monitorInfo;
                    Global.CurrentMonitor = Global.UpdateMonitorInfo(monitorInfo.Value);
                }
            }
        }

        public static void BringWindowToFront(IntPtr hwnd)
        {
            SetForegroundWindow(hwnd);
        }

        public static void BringWindowAndItsGroupToFront(IntPtr hwnd)
        {
            var gs = Global.ForegroundGroups
                .Where(p => p.Windows.Any(w => w.Hwnd == hwnd))
                .ToList();
            if (gs != null && gs.Count > 0)
            {
                var index = 0;
                var g = gs[index];
                //StaticStorage.IsPauseEventChecking = true;
                Console.WriteLine("start fgs with {0:x8}", hwnd.ToInt32());
                foreach (var w in g.Windows)
                {
                    //if (w.Hwnd == hwnd) continue;
                    Console.WriteLine("restore {0:x8}", w.Hwnd.ToInt32());
                    /*ignoredNextFgEvent.Add(w.Hwnd);
                    if(IsIconic(w.Hwnd))
                    {
                        ShowWindow(w.Hwnd, CmdShow.SW_RESTORE);
                    }*/
                    ShowWindow(w.Hwnd, CmdShow.SW_RESTORE);
                    SetWindowPos(w.Hwnd, HWND_TOPMOST, 0, 0, 0, 0, 
                        WindowPosFlags.IgnoreResizeAndMove);
                    SetWindowPos(w.Hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, 
                        WindowPosFlags.IgnoreResizeAndMove);

                    //ShowWindowAsync(new HandleRef(null, hwnd), CmdShow.SW_RESTORE);
                    //SetForegroundWindow(w.Hwnd);
                    //anchor = w.Hwnd;
                }
                //SetForegroundWindow(hwnd);

                /*new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Console.WriteLine("start fgs with {0:x8}", hwnd.ToInt32());
                    foreach (var w in g.Windows)
                    {
                        if (w.Hwnd == hwnd) continue;
                        Console.WriteLine("restore {0:x8}", w.Hwnd.ToInt32());
                        ShowWindow(w.Hwnd, CmdShow.SW_RESTORE);
                        SetForegroundWindow(w.Hwnd);
                        anchor = w.Hwnd;
                    }
                    SetForegroundWindow(hwnd);
                }).Start();*/
                //anchor = hwnd;
                //ShowWindow(hwnd, CmdShow.SW_RESTORE);
                //SetForegroundWindow(hwnd);
            }
        }

        static void WinEventProc(IntPtr hWinEventHook, EventType eventType,
            IntPtr hwnd, WinEventObjectId idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            /*if (hwnd.ToInt32().ToString("x8") != "00201e1c") return;

            //if (hwnd.ToInt32().ToString("x8") == "00201e1c")
            {
               *//* Console.WriteLine("{6:x8} {0} {1} {2} {3} {4} {5}",
                    hWinEventHook, eventType*//*.ToString("x")*//*, idObject, idChild, dwEventThread, dwmsEventTime,
                    hwnd.ToInt32());*//*
            }*/

            if(PauseWinEventHook) return;

            if (idObject == WinEventObjectId.OBJID_CURSOR || idObject == WinEventObjectId.OBJID_CARET)
            {
                return;
            }
            if (hwnd == IntPtr.Zero)
            {
                return;
            }
            //Console.WriteLine("{0:x} - {1:x8} - {2}", eventType, hwnd.ToInt32(), idObject);
            if (idObject != WinEventObjectId.OBJID_SELF)
            {
                return;
            }

            /*if (hwnd.ToInt32().ToString("x8") != "002e08e8")
            {
                return;
            }*/

            //Console.WriteLine("{0} {1} {2} {3} {4} {5}", hWinEventHook, eventType, idObject, idChild, dwEventThread, dwmsEventTime);

            /*if (ignoredNextFgEvent.Contains(hwnd) && eventType == EventType.EVENT_SYSTEM_FOREGROUND)
            {
                Console.WriteLine("{0:x8} ignored fge", hwnd.ToInt32());
                ignoredNextFgEvent.Remove(hwnd);
                foreground = hwnd;
                return;
            }*/

            /*if (anchor == hwnd && StaticStorage.IsPauseEventChecking
                && eventType == EventType.EVENT_SYSTEM_FOREGROUND)
            {
                StaticStorage.IsPauseEventChecking = false;
                Console.WriteLine("end fgs with {0:x8}", hwnd.ToInt32());
                foreground = hwnd;
                return;
            }

            if (StaticStorage.IsPauseEventChecking)
            {
                return;
            }*/
            //Console.WriteLine("{0:x}", eventType);

            /*WindowPlacement wp2 = new WindowPlacement();
            GetWindowPlacement(hwnd, ref wp2);
            Console.WriteLine("{0:x} - {1}", eventType, CmdShow.SW_MAXIMIZE == wp2.showCmd);*/

            //Console.WriteLine("{0:x}", idChild);

            switch (eventType)
            {
                /*case EventType.EVENT_OBJECT_INVOKED:
                    Console.WriteLine("invoked {0:x8}", hwnd.ToInt32());
                    break;*/
                /*case EventType.EVENT_OBJECT_REORDER:
                    if(idObject == WinEventObjectId.OBJID_CLIENT)
                    {
                        Utils.Timer.StartTimer("Start detect/move window to layout");
                        SetDesktopAndMonitor(hwnd);
                        Console.WriteLine("{0:x8} is maximized", hwnd.ToInt32());
                        if(currentMonitor != null)
                        {
                            Point lpPoint;
                            GetCursorPos(out lpPoint);
                            var layout = Global.GetMonitorLayout(null, lpPoint, 
                                currentMonitor.Value.DeviceName, Global.CurrentDesktopName);
                            if(layout != null)
                            {
                                //if(layout.Width != 100 || layout.Height != 100)
                                {
                                    SnapWindowToLayout(hwnd, Global.CurrentMonitor, layout);
                                }

                                // Tab button
                                var tbkey = CreateTabButton(hwnd, Global.CurrentMonitor, layout,
                                    Global.CurrentDesktopName);
                            }
                        }
                        Utils.Timer.StopTimer("End detect/move window to layout");
                        break;
                    }
                    break;*/
                case EventType.EVENT_OBJECT_DESTROY:
                    if (Global.TabButtonWindows.ContainsKey(hwnd))
                    {
                        MethodInvoker action = delegate {
                            var tab = Global.TabButtonWindows[hwnd];
                            tab.Parent.RemoveTabWindowButton(tab);
                            Global.TabButtonWindows.Remove(hwnd);
                        };
                        Global.Main.BeginInvoke(action);
                    }
                    /*Console.WriteLine("destroyed {0:x8} {1} {2}", hwnd.ToInt32(), 
                        idObject, idChild);*/
                    break;
                /*case EventType.EVENT_SYSTEM_DESKTOPSWITCH:
                    Console.WriteLine("desktop switch {0:x8}", hwnd.ToInt32());
                    break;
                case EventType.EVENT_OBJECT_INVOKED:
                    Console.WriteLine("invoked {0:x8}", hwnd.ToInt32());
                    break;
                */
                case EventType.EVENT_OBJECT_NAMECHANGE:
                    /*StringBuilder sb = new StringBuilder(100);
                    GetClassName(hwnd, sb, 100);
                    Console.WriteLine("namechanged {0:x8} {1} {2} {3}", hwnd.ToInt32(), 
                            idObject, idChild, sb);
                    Rect rcClient = new Rect();
                    Rect rcWind = new Rect();
                    GetClientRect(hwnd, ref rcClient); 
                    GetWindowRect(hwnd, ref rcWind);
                    \Console.WriteLine(rcClient);
                    Console.WriteLine(rcWind);*/
                    if (Global.TabButtonWindows.ContainsKey(hwnd))
                    {
                        var tab = Global.TabButtonWindows[hwnd];
                        if (tab.TitleEvent)
                        {
                            tab.Text = GetWindowTitle(hwnd);
                        }
                    }
                    /*if(Global.Settings.WindowEventListeners
                        .Where(p => p.Type == "title" && p.Hwnds.Contains(hwnd)).Any()){
                        Console.WriteLine("namechanged {0:x8} {1} {2}", hwnd.ToInt32(), 
                            idObject, idChild);
                    }*/
                    /*if (StaticStorage.TabButtonOfHwnds.ContainsKey(hwnd))
                    {
                        StaticStorage.TabButtonOfHwnds[hwnd].Text = GetWindowTitle(hwnd);
                    }*/
                    break;
                /*case EventType.EVENT_OBJECT_STATECHANGE:
                    Console.WriteLine("statechange {0:x8}", hwnd.ToInt32());
                    break;*/
                /*case EventType.EVENT_SYSTEM_MOVESIZESTART:
                    Console.WriteLine("movesizestart {0:x8}", hwnd.ToInt32());
                    break;*/
                case EventType.EVENT_SYSTEM_FOREGROUND:
                    Console.WriteLine("foreground {0:x8} {1}", hwnd.ToInt32(), GetProcessFileName(hwnd));
                    if(IsTopLevelWindows(hwnd))
                    {
                        foreground = hwnd;
                        if (Global.IsDetectWindow)
                        {
                            var title = GetWindowTitle(hwnd);
                            if(title != "ZWindows")
                            {
                                if (!string.IsNullOrEmpty(title))
                                {
                                    Global.CurrentForegroundWindow.Hwnd = hwnd;
                                    Global.CurrentForegroundWindow.Name = title;
                                    Global.CurrentForegroundWindowText.Text = title;
                                }
                                else
                                {
                                    Global.CurrentForegroundWindow.Hwnd = IntPtr.Zero;
                                    Global.CurrentForegroundWindow.Name = "";
                                    Global.CurrentForegroundWindowText.Text = "Could not detect window!";
                                }
                            }
                        }
                        else
                        {
                            // BringWindowsToFront(hwnd);
                            if (Global.TabButtonWindows.ContainsKey(hwnd))
                            {
                                var tab = Global.TabButtonWindows[hwnd];
                                tab.Parent.SelectedWindowTabButton = tab;
                            }
                        }
                    }
                    break;
                case EventType.EVENT_OBJECT_LOCATIONCHANGE:
                    WindowPlacement wp = new WindowPlacement();
                    GetWindowPlacement(hwnd, ref wp);
                    if (CmdShow.SW_MAXIMIZE == wp.showCmd)
                    {
                        PauseWinEventHook = true;
                        Utils.Timer.StartTimer("Start detect/move window to layout");
                        if(IsTopLevelWindows(hwnd))
                        {
                            SetDesktopAndMonitor(hwnd);
                            Console.WriteLine("{0:x8} is maximized", hwnd.ToInt32());
                            if (currentMonitor != null)
                            {
                                Point lpPoint;
                                GetCursorPos(out lpPoint);
                                var layout = Global.GetMonitorLayout(null, lpPoint,
                                    Global.GetMonitorId(currentMonitor.Value), Global.CurrentDesktopName);
                                if (layout != null)
                                {
                                    SnapWindowToLayout(hwnd, Global.CurrentMonitor, layout);

                                    MethodInvoker action = delegate {
                                        var tbkey = CreateOrUpdateTabButton(hwnd, Global.CurrentMonitor, layout);
                                    };
                                    Global.Main.BeginInvoke(action);
                                    //Global.TabButtonsBars[tbkey].Visible = true;
                                }
                            }
                            Utils.Timer.StopTimer("End detect/move window to layout");
                        }
                        PauseWinEventHook = false;
                        break;
                    }
                    break;
                case EventType.EVENT_SYSTEM_MINIMIZESTART:
                    if (Global.IgnoredMinimizeEvents.Contains(hwnd))
                    {
                        Global.IgnoredMinimizeEvents.Remove(hwnd);
                        break;
                    }
                    if(Global.Settings.DisabledMinimize)
                    {
                        if(hwnd == foreground){
                            Console.WriteLine("{0:x8} is restoring", hwnd.ToInt32());
                            ShowWindow(hwnd, CmdShow.SW_RESTORE);
                        }
                    }
                    break;
                case EventType.EVENT_SYSTEM_MINIMIZEEND:
                    Console.WriteLine("{0:x8} is minimized", hwnd.ToInt32());
                    break;
            }
        }

        /// <summary>
        /// Create TabBar (if not exits)
        /// Create TabButton (if not exits). Add/Move TabButton into TabBar
        /// TabBar Id = vdn + layout x + layout y;
        /// </summary>
        public static string CreateOrUpdateTabButton(IntPtr hwnd, MonitorDevice monitor, 
            MonitorLayout layout)
        {
            if(!layout.TabsBar.Enable) return null;
            var title = Win32Helper.GetWindowTitle(hwnd);
            if(string.IsNullOrEmpty(title)) return null;

            var tbkey = (monitor.X + layout.Left).ToString() 
                + ":" + (monitor.Y + layout.Top).ToString()
                + ":" + layout.Width.ToString();

            if (!Global.TabButtonsBars.ContainsKey(tbkey))
            {
                var l = monitor.X + layout.Left + layout.TabsBar.LeftOffset;
                var y = monitor.Y + layout.Top;
                //var layoutIndex = monitor.Layouts.IndexOf(layout);
                Global.TabButtonsBars[tbkey] = new ScreenTabButtonsBar(null, layout.TabsBar)
                {
                    Key = tbkey,
                    MaxWidth = layout.TabsBar?.MaxWidth > 0 ? layout.TabsBar.MaxWidth : layout.CWidth,
                    X = l,
                    Y = y,
                    Location = new System.Drawing.Rectangle
                    {
                        X = l,
                        Y = y
                    },
                    Visible = true
                };
            }
            var bar = Global.TabButtonsBars[tbkey];

            Console.WriteLine($"bar maxwidth: {bar.MaxWidth}");

            WindowTabButton tab = Global.TabButtonWindows.ContainsKey(hwnd) 
                ? Global.TabButtonWindows[hwnd] : null;
            if(tab != null)
            {
                if(tab.Parent == bar)
                {
                    return null;
                }
                else
                {
                    tab.Parent.RemoveTabWindowButton(hwnd);
                    bar.AddTabWindowButton(tab);
                }
            }
            else
            {
                tab = bar.AddTabWindowButton(hwnd, title);
                Global.TabButtonWindows[hwnd] = tab;
            }
            tab.Layout = layout;
            return tbkey;
        }

        public static void SnapWindowToLayout(IntPtr hwnd, MonitorDevice monitor, MonitorLayout layout)
        {
            if(monitor == null)
            {
                SetDesktopAndMonitor(hwnd);
                monitor = Global.CurrentMonitor;
            }
            ShowWindow(hwnd, CmdShow.SW_RESTORE);
            var border_thickness = GetBorderSize(hwnd);

            // TODO
            var mdx = monitor.X > 0 ? 1 : 0;
            var mdy = monitor.Y > 0 ? 1 : 0;
            mdx = 0;
            mdy = 0;

            Console.WriteLine("pos: {0} {1} {2} {3}", 
                monitor.X + mdx + layout.Left - border_thickness + layout.LeftOffset, 
                monitor.Y + mdy + layout.Top + layout.TopOffset, 
                -mdx * 1 + layout.CWidth + border_thickness * 2 + layout.RightOffset - layout.LeftOffset, 
                -mdy * 1 + layout.CHeight + border_thickness + layout.BottomOffset - layout.TopOffset);

            SetWindowPos(hwnd, IntPtr.Zero, 
                monitor.X + mdx + layout.Left - border_thickness + layout.LeftOffset, 
                monitor.Y + mdy + layout.Top + layout.TopOffset, 
                -mdx * 1 + layout.CWidth + border_thickness * 2 + layout.RightOffset - layout.LeftOffset, 
                -mdy * 1 + layout.CHeight + border_thickness + layout.BottomOffset - layout.TopOffset, 
                WindowPosFlags.ShowWindow);
        }

        public static int GetBorderSize(IntPtr hwnd)
        {
            Rect rcClient = new Rect();
            Rect rcWind = new Rect();
            GetClientRect(hwnd, ref rcClient); 
            GetWindowRect(hwnd, ref rcWind);
            Console.WriteLine("Rect of {0:x8}", hwnd.ToInt32());
            Console.WriteLine(rcClient);
            Console.WriteLine(rcWind);
            return ((rcWind.Right - rcWind.Left) - rcClient.Right) / 2;
        }

        public static String GetWindowTitle(IntPtr hwnd)
        {
            StringBuilder title = new StringBuilder(1024);
            GetWindowText(hwnd, title, title.Capacity);
            return title.ToString();
        }

        public static void RefreshTabBars()
        {
            Utils.Timer.StartTimer();
            foreach(var b in Global.TabButtonsBars.Values)
            {
                b.Dispose();
            }
            Global.TabButtonsBars.Clear();
            Global.TabButtonWindows.Clear();


            var windows = Global.GetDesktopWindows();
            //var vdn = Global.VD.GetDesktopName(Global.VD.GetCurrentDesktopId());
            foreach(var window in windows)
            {
                Console.WriteLine("------");
                Console.WriteLine("{0:x8} {1} v{2} c{3}", window.Handle.ToInt32(), window.Title,
                    window.IsVisible, window.IsIconic);

                Rect rcClient = new Rect();
                GetClientRect(window.Handle, ref rcClient);
                if(rcClient.Left == 0 && rcClient.Right == 0 
                    && rcClient.Top == 0 && rcClient.Bottom == 0)
                {
                    continue;
                }
                Rect rcWind = new Rect();
                GetWindowRect(window.Handle, ref rcWind);
                //Console.WriteLine(rcClient);
                //Console.WriteLine(rcWind);

                if (rcWind.Left <= -10000 || rcWind.Top <= -10000)
                {
                    continue;
                }

                //var borderSize = ((rcWind.Right - rcWind.Left) - rcClient.Right) / 2;
                var x = rcWind.Left;
                var y = rcWind.Top;
                var w = rcWind.Right - rcWind.Left;
                var h = rcWind.Bottom - rcWind.Top;
                var delta = Global.Settings.DeltaForAutoDetectWindowLayout;

                Console.WriteLine($"x: {x}, y: {y}, w: {w}, h: {h}");

                var availableMonitorIds = Global.MonitorDevices.Select(p => p.Id).ToList();
                var availableMonitors = Global.Settings.MonitorsLayouts.Values
                    .Where(p => availableMonitorIds.Contains(p.Id))
                    .ToList();

                foreach(var monitor in availableMonitors)
                {
                    var mx = x - monitor.X;
                    var my = y - monitor.Y;

                    Console.WriteLine($"{monitor.Name}, {mx}, {my}");

                    var layout = monitor.Layouts.Where(p => 
                        p.TabsBar.Enable 
                        //&& p.DesktopName == vdn
                        && (mx >= p.Left - delta && mx <= p.Left + delta)
                        && (my >= p.Top - delta && my <= p.Top + delta)
                        && (w >= p.CWidth - delta && w <= p.CWidth + delta)
                        && (h >= p.CHeight - delta && h <= p.CHeight + delta)
                    ).FirstOrDefault();
                    if(layout != null)
                    {
                        Console.WriteLine($"layout#{monitor.Layouts.IndexOf(layout)}, {layout.Left}, {layout.Top}, {layout.CWidth}, {layout.CHeight}");
                        var tbkey = CreateOrUpdateTabButton(window.Handle, monitor, layout);
                    }
                }
            }

            //Global.TabButtonsBars.Values.ToList().ForEach(p => p.Visible = true);
            Utils.Timer.StopTimer("GetProccesses");
        }

        public static void SaveDesktop()
        {
            Utils.Timer.StartTimer();
            var windows = Global.GetDesktopWindows();
            var vdn = Global.CurrentDesktopName;
            var ds = new DesktopState { DesktopName = vdn, WindowStates = new List<WindowState>() };

            foreach(var window in windows)
            {
                Rect rcClient = new Rect();
                GetClientRect(window.Handle, ref rcClient);
                if(rcClient.Left == 0 && rcClient.Right == 0 
                    && rcClient.Top == 0 && rcClient.Bottom == 0)
                {
                    continue;
                }
                Rect rcWind = new Rect();
                GetWindowRect(window.Handle, ref rcWind);

                if(rcWind.Left <= -10000 || rcWind.Top <= -10000)
                {
                    continue;
                }

                Console.WriteLine("-----------");
                Console.WriteLine(rcClient);
                Console.WriteLine(rcWind);

                var borderSize = ((rcWind.Right - rcWind.Left) - rcClient.Right) / 2;
                var x = rcWind.Left;
                var y = rcWind.Top;
                var w = rcClient.Right + borderSize * 2;
                var h = rcClient.Bottom + borderSize;

                var ws = new WindowState
                {
                    Hwnd = window.Handle,
                    X = x,
                    Y = y,
                    Width = w,
                    Height = h
                };
                ds.WindowStates.Add(ws);
            }
            Global.DesktopStates[vdn] = ds;
            File.WriteAllText(Global.GetDesktopsStatesFilePath(), 
                JsonConvert.SerializeObject(Global.DesktopStates, Formatting.Indented));
            Utils.Timer.StopTimer("SaveWindowsStatesOfCurrentDesktop");
        }
        
        public static void LoadDesktop(string vdn = null)
        {
            if(vdn == null)
            {
                vdn = Global.CurrentDesktopName;
            }
            if(Global.DesktopStates.Keys.Count == 0)
            {
                Global.DesktopStates = Global.LoadDesktopsStatesFromFile();
            }
            if (Global.DesktopStates.ContainsKey(vdn))
            {
                var ds = Global.DesktopStates[vdn];
                foreach(var ws in ds.WindowStates)
                {
                    if (IsIconic(ws.Hwnd))
                    {
                        ShowWindow(ws.Hwnd, CmdShow.SW_RESTORE);
                    }
                    SetWindowPos(ws.Hwnd, IntPtr.Zero, 
                        ws.X, 
                        ws.Y, 
                        ws.Width, 
                        ws.Height, 
                        WindowPosFlags.ShowWindow);
                }
            }
            RefreshTabBars();
        }
        
        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);
        public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(EventType eventMin, EventType eventMax, IntPtr
           hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
           uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, CmdShow nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            WindowPosFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(
            IntPtr hWnd, ref WindowPlacement lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hwnd, ref Rect lpRect);

        [DllImport("user32.dll")]
        static extern bool AdjustWindowRectEx(ref Rect lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll")]
        static extern bool ShowWindowAsync(HandleRef hWnd, CmdShow nCmdShow);

        [DllImport("user32", EntryPoint = "SetWindowsHookExA")]
        static extern IntPtr SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hmod, IntPtr dwThreadId);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
            ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction,
            IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, EnumWindowProc callback, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)]
            Boolean bInheritHandle,
            Int32 dwProcessId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryFullProcessImageName([In]IntPtr hProcess, [In]int dwFlags, [Out]StringBuilder lpExeName, ref int lpdwSize);
        
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr parentHwnd, Point Point);

        [DllImport("user32.dll")]
        public static extern IntPtr RealChildWindowFromPoint(IntPtr parentHwnd, Point Point);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetClassName(IntPtr hWnd, 
            System.Text.StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, uint nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();


        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                Win32Helper.EnumWindowProc childProc = new Win32Helper.EnumWindowProc(EnumWindow);
                Win32Helper.EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            try
            {
                if (IntPtr.Size == 4)
                    return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
                else
                    return GetClassLongPtr64(hWnd, nIndex);
            }
            catch(Exception ex)
            {
                return IntPtr.Zero;
            }
        }

        public static Process GetModernAppProcessPath(IntPtr hwnd) {
            uint pid = 0;
            Win32Helper.GetWindowThreadProcessId(hwnd, out pid);            
            // now this is a bit tricky. Modern apps are hosted inside ApplicationFrameHost process, so we need to find
            // child window which does NOT belong to this process. This should be the process we need
            var children = Win32Helper.GetChildWindows(hwnd);
            foreach (var childHwnd in children) {
                uint childPid = 0;
                Win32Helper.GetWindowThreadProcessId(childHwnd, out childPid);
                if (childPid != pid) {
                    // here we are
                    return Process.GetProcessById((int) childPid);
                }
            }
            return null;
            //throw new Exception("Cannot find a path to Modern App executable file");
        }

        public static string GetProcessFileName(IntPtr hwnd)
        {
            var p = GetProcess(hwnd);
            if(p != null)
            {
                return p.ProcessName + "|" + p.MainModule.FileName;
            }
            return null;
        }

        public static Process GetProcess(IntPtr hwnd)
        {
            try
            {
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                Process p = Process.GetProcessById((int)pid);
                if (p.MainModule.FileName.IndexOf("ApplicationFrameHost.exe") != -1)
                {
                    p = GetModernAppProcessPath(hwnd);
                }
                return p;
            }
            catch
            {
                
            }
            return null;
        }

        public static bool IsMatch(IntPtr hwnd, WindowFilterRule rule)
        {
            var p = GetProcess(hwnd);
            if(rule.ExactProcessName != null && rule.ExactProcessName == p.ProcessName)
            {
                return true;
            }
            if(rule.ExactExeFileName != null)
            {
                var exefilename = p.MainModule.FileName.Substring(p.MainModule.FileName.LastIndexOf('/'));
                if(rule.ExactExeFileName == exefilename)
                {
                    return true;
                }
            }
            if(rule.ExactTitle != null && p.MainWindowTitle == rule.ExactTitle)
            {
                return true;
            }
            if(rule.RegexTitle != null)
            {
                if(Regex.IsMatch(p.MainWindowTitle, rule.RegexTitle))
                {
                    return true;
                }
            }
            return false;
        }

















        public static void EnableCloseButton(IntPtr hwnd, bool enabled) {
            IntPtr hMenu;
            int n;
            hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero) {
                n = GetMenuItemCount(hMenu);
                if (n > 0) {
                    if (enabled) {
                        EnableClose(hwnd);
                    }
                    else {
                        DisableClose(hwnd);
                    }
                    SendMessage(hwnd, WM_NCACTIVATE, (IntPtr)1, (IntPtr)0);
                    DrawMenuBar(hwnd);
                    Application.DoEvents();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public int wID;
            public int hSubMenu;
            public int hbmpChecked;
            public int hbmpUnchecked;
            public int dwItemData;
            public string dwTypeData;
            public uint cch;
            //    public int hbmpItem;
        }

        internal const int MF_BYCOMMAND = 0x00000000;
        internal const UInt32 SC_CLOSE = 0xF060;
        internal const UInt32 SC_MAXIMIZE = 0xF030;
        internal const UInt32 SC_MINIMIZE = 0xF020;

        //SetMenuItemInfo fMask constants.
        const UInt32 MIIM_STATE = 0x1;
        const UInt32 MIIM_ID = 0x2;

        //'SetMenuItemInfo fState constants.
        const UInt32 MFS_ENABLED = 0x0;
        const UInt32 MFS_GRAYED = 0x3;
        const UInt32 MFS_CHECKED = 0x8;

        internal const int MFS_DEFAULT = 0x1000;

        [DllImport("user32.dll")]
        static extern bool SetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        static extern bool GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        public enum GetAncestorFlags
        {
            GetParent = 1,
            GetRoot = 2,
            GetRootOwner = 3
        }

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);


        private const UInt32 WM_NCACTIVATE = 0x0086;

        private static void DisableClose(IntPtr hwnd) {
            IntPtr hMenu;
            int n;
            hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero) {
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));
                mif.fMask = MIIM_ID | MIIM_STATE;
                mif.fType = 0;
                mif.dwTypeData = null;
                bool a = GetMenuItemInfo(hMenu, (int)SC_CLOSE, false, ref mif);
                mif.fState = MFS_GRAYED;
                SetMenuItemInfo(hMenu, (int)SC_CLOSE, false, ref mif);
                SendMessage(hwnd, WM_NCACTIVATE, (IntPtr)1, (IntPtr)0);

                mif.wID = -10;
                mif.fState = MFS_GRAYED;
                SetMenuItemInfo(hMenu, (int)SC_CLOSE, false, ref mif);
            }
        }

        private static void EnableClose(IntPtr hwnd) {
            IntPtr hMenu;
            int n;
            hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero) {
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));
                mif.fMask = MIIM_ID | MIIM_STATE;
                mif.fType = 0;
                mif.dwTypeData = null;
                bool a = GetMenuItemInfo(hMenu, -10, false, ref mif);
                mif.wID = (int)SC_CLOSE;
                SetMenuItemInfo(hMenu, -10, false, ref mif);
                SendMessage(hwnd, WM_NCACTIVATE, (IntPtr)1, (IntPtr)0);

                mif.fState = MFS_ENABLED;
                SetMenuItemInfo(hMenu, (int)SC_CLOSE, false, ref mif);
                SendMessage(hwnd, WM_NCACTIVATE, (IntPtr)1, (IntPtr)0);
            }
        }

        public static IntPtr GetRealParent(IntPtr hWnd)
        {
            IntPtr hParent;

            hParent = GetAncestor(hWnd, GetAncestorFlags.GetParent);
            if (hParent.ToInt64() == 0 || hParent == GetDesktopWindow())
            { 
                hParent = GetParent(hWnd);
                if (hParent.ToInt64() == 0 || hParent == GetDesktopWindow())
                { 
                    hParent = hWnd;
                }

            }

            return hParent;
        }

        public static bool IsTopLevelWindows(IntPtr hWnd)
        {
            IntPtr hParent;

            hParent = GetAncestor(hWnd, GetAncestorFlags.GetParent);
            if(hParent == GetDesktopWindow())
            {
                return true;
            }
            else if (hParent.ToInt64() == 0)
            { 
                hParent = GetParent(hWnd);
                if(hParent == GetDesktopWindow())
                {
                    return true;
                }
            }
            return false;
        }

        [DllImport("user32.dll", SetLastError=true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOPMOST = 0x0008;

        public static bool IsWindowTopMost(IntPtr hWnd)
        {
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            return (exStyle & WS_EX_TOPMOST) == WS_EX_TOPMOST;
        }
    }
}
