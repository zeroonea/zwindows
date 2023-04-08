using System;
using zwindowscore.Enum;
using zwindowscore.Struct;
using System.Windows.Forms;
using zwindowscore.Utils.UI;
using static zwindowscore.Monitor;
using zwindowscore.Options;
using System.Diagnostics;
using System.Threading;

namespace zwindowscore
{
    public static partial class Win32Helper
    {
        public static bool PauseWinEventHook = false;
        static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);
        static IntPtr foreground;
        static MonitorDeviceInfo currentMonitor;
        static Guid? currentDesktopId;

        static int DragWindowState = 0;
        static IntPtr DragWindowHwnd;
        static Rect DragWindowRect;
        static MonitorLayout DragWindowLayout;
        
        public static void StartEventHook()
        {
            IntPtr hhook = SetWinEventHook(EventType.EVENT_MIN, 
                    EventType.EVENT_MAX, 
                    IntPtr.Zero,
                    procDelegate, 0, 0, 
                    WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
        }

        static void WinEventProc(IntPtr hWinEventHook, EventType eventType,
            IntPtr hwnd, WinEventObjectId idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (PauseWinEventHook 
                || idObject == WinEventObjectId.OBJID_CURSOR 
                || idObject == WinEventObjectId.OBJID_CARET
                || hwnd == IntPtr.Zero
                || idObject != WinEventObjectId.OBJID_SELF 
                || idChild != 0)
            {
                return;
            }
            var tab = Global.FindTabButtonWithHwnd(hwnd);
            //Debug.WriteLine("foreground {2} {0:x8} {1}", hwnd.ToInt32(), GetProcessFileName(hwnd), eventType);
            switch (eventType)
            {
                //case EventType.WM_WINDOWPOSCHANGING:
                //    Debug.WriteLine("foreground {0:x8} {1}", hwnd.ToInt32(), GetProcessFileName(hwnd));
                //    break;
                case EventType.EVENT_OBJECT_DESTROY:
                    if (tab != null)
                    {
                        MethodInvoker action = delegate {
                            if(tab.Parent != null)
                            { 
                                tab.Parent.RemoveTabWindowButton(tab);
                            }
                            Global.TabButtonWindows.Remove(hwnd);
                        };
                        Global.Main.BeginInvoke(action);
                    }
                    break;
                
                case EventType.EVENT_OBJECT_NAMECHANGE:
                    if (tab != null)
                    {
                        if (tab.TitleEvent)
                        {
                            tab.Text = GetWindowTitle(hwnd);
                        }
                    }
                    break;
                
                case EventType.EVENT_SYSTEM_FOREGROUND:
                    //Debug.WriteLine("foreground {0:x8} {1}", hwnd.ToInt32(), GetProcessFileName(hwnd));
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
                            if (tab != null && tab.Parent != null)
                            {
                                tab.Parent.SelectedWindowTabButton = tab;
                            }
                        }
                    }
                    break;
                case EventType.EVENT_OBJECT_LOCATIONCHANGE:
                    if(DragWindowState == 0)
                    { 
                        WindowPlacement wp = new WindowPlacement();
                        GetWindowPlacement(hwnd, ref wp);
                        if (CmdShow.SW_MAXIMIZE == wp.showCmd)
                        {
                            SnapWindow(hwnd);
                            break;
                        }
                    }
                    else if(!Global.Settings.DisableDragWindowOverlay)
                    { 
                        if(DragWindowState == 0 || DragWindowHwnd != hwnd) break;

                        if(DragWindowState == 1)
                        {
                            //Debug.WriteLine("Check drag or reisze...");

                            var tmp = new Rect();
                            GetClientRect(hwnd, ref tmp);
                            if(tmp.ToString() == DragWindowRect.ToString())
                            {
                                DragWindowState = 2; //dragging
                                //Debug.WriteLine("it is dragging");
                            }
                            else
                            {
                                DragWindowState = 3; //resizing
                                //Debug.WriteLine("it is resizing");
                            }
                        }
                        else if(DragWindowState == 2)
                        {
                            //Debug.WriteLine("dragging");
                            Point lpPoint;
                            GetCursorPos(out lpPoint);
                            var layout = Global.GetMonitorLayout(null, lpPoint, 
                                null, Global.CurrentDesktopName);
                            if (layout != null)
                            {
                                DragWindowLayout = layout;
                                MethodInvoker action = delegate
                                {
                                    //Debug.WriteLine("set layout highlight");
                                    //PauseWinEventHook = true;
                                    LayoutOverlay.ShowOverlay(hwnd,
                                        layout._monitor.X + layout.Left,
                                        layout._monitor.Y + layout.Top,
                                        layout.CWidth,
                                        layout.CHeight);
                                    //PauseWinEventHook = false;
                                    //Debug.WriteLine("end layout highlight");
                                };
                                Global.Main.BeginInvoke(action);
                            }
                        }
                    }
                    break;
                case EventType.EVENT_SYSTEM_MINIMIZESTART:
                    if (Global.IgnoredMinimizeEvents.Contains(hwnd))
                    {
                        Global.IgnoredMinimizeEvents.Remove(hwnd);
                        break;
                    }
                    if (Global.Settings.DisabledMinimize)
                    {
                        if (hwnd == foreground)
                        {
                            //Debug.WriteLine("{0:x8} is restoring", hwnd.ToInt32());
                            ShowWindow(hwnd, CmdShow.SW_RESTORE);
                        }
                    }
                    break;
                //case EventType.EVENT_SYSTEM_MINIMIZEEND:
                //    Debug.WriteLine("{0:x8} is minimized", hwnd.ToInt32());
                //    break;

                case EventType.EVENT_SYSTEM_MOVESIZESTART:
                    if(!Global.Settings.DisableDragWindowOverlay)
                    {
                        if(DragWindowState == 0)
                        {
                            //Debug.WriteLine("Begin Drag");
                            DragWindowHwnd = hwnd;
                            DragWindowState = 1;
                            DragWindowRect = new Rect();
                            GetClientRect(hwnd, ref DragWindowRect);
                        }
                    }
                    break;

                case EventType.EVENT_SYSTEM_MOVESIZEEND:
                    if(!Global.Settings.DisableDragWindowOverlay)
                    { 
                        if(DragWindowState == 2)
                        {
                            if(DragWindowHwnd == hwnd && DragWindowLayout != null)
                            {
                                MethodInvoker action = delegate
                                {
                                    LayoutOverlay.ClearOverlay();

                                    // The multi tabs app like chrome, edge, firefox auto snap when we move single chrome tab
                                    // to another, we need to wait a little to the auto snap done
                                    // when it done, the current windows (single chrome tab that we moving) will be seen as
                                    // not root owner windows > clean up tab button
                                    Thread.Sleep(Global.Settings.MultiTabsBugThreadSleep);
                                    
                                    if (!Global.Settings.DisableAutoSnapWindowAfterDrag)
                                    {
                                        //Debug.WriteLine(hwnd);
                                        if (SnapWindowToLayout(hwnd,
                                            DragWindowLayout._monitor, DragWindowLayout, true))
                                        {
                                            //Debug.WriteLine("true " + hwnd);
                                            CreateOrUpdateTabButton(hwnd,
                                                DragWindowLayout._monitor, DragWindowLayout);
                                        }
                                    }
                                };
                                Global.Main.BeginInvoke(action);
                            }
                        }

                        DragWindowState = 0;
                    }
                    break;
            }
        }
    }
}
