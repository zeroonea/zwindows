using System;
using System.Runtime.InteropServices;
using zwindowscore.Enum;
using zwindowscore.Struct;
using System.Linq;
using System.Text;
using zwindowscore.Utils;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using zwindowscore.Options;

namespace zwindowscore
{
    public static partial class Win32Helper
    {
        public static string GetDesktopName(Guid? desktopId, int index = 0)
        {
            if (desktopId == null) return null;
            string desktopName = null;
            try
            {
                desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktops\\Desktops\\{" + desktopId.ToString() + "}", "Name", null);
            }
            catch  { }
            if (string.IsNullOrEmpty(desktopName))
            {
                return $"Unnamed-{index}";
            }
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
                if(currentMonitor == null || currentMonitor.DeviceName != monitorInfo.DeviceName)
                {
                    Console.WriteLine("Monitor {0}: {1}x{2}, {3}x{4}", 
                        monitorInfo?.DeviceName, 
                        monitorInfo?.GetRealWidth(), 
                        monitorInfo?.GetRealHeight(), 
                        monitorInfo?.GetWidth(),
                        monitorInfo?.GetHeight());

                    currentMonitor = monitorInfo;
                    Global.CurrentMonitor = Global.UpdateMonitorInfo(monitorInfo);
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
                Console.WriteLine("start fgs with {0:x8}", hwnd.ToInt32());
                foreach (var w in g.Windows)
                {
                    ShowWindow(w.Hwnd, CmdShow.SW_RESTORE);
                    SetWindowPos(w.Hwnd, HWND_TOPMOST, 0, 0, 0, 0, 
                        WindowPosFlags.IgnoreResizeAndMove);
                    SetWindowPos(w.Hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, 
                        WindowPosFlags.IgnoreResizeAndMove);
                }
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
                if(Global.Settings.NoTabBarDesktopNames != null 
                    && Global.Settings.NoTabBarDesktopNames.Contains(Global.CurrentDesktopName))
                {
                    Global.TabButtonsBars[tbkey].Bar.Hide();
                }
                //Global.TabButtonsBars[tbkey].Opacity = 0.8;
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
            PauseWinEventHook = true;

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

            PauseWinEventHook = false;
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

        private static void DisableClose(IntPtr hwnd) {
            IntPtr hMenu;
            int n;
            hMenu = GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero) {
                MenuItemInfo mif = new MenuItemInfo();
                mif.cbSize = (uint)Marshal.SizeOf(typeof(MenuItemInfo));
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
                MenuItemInfo mif = new MenuItemInfo();
                mif.cbSize = (uint)Marshal.SizeOf(typeof(MenuItemInfo));
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

        public static bool IsWindowTopMost(IntPtr hWnd)
        {
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            return (exStyle & WS_EX_TOPMOST) == WS_EX_TOPMOST;
        }
    }
}
