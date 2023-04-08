using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TaskbarTool.Constants;
using TaskbarTool.Structs;
using TaskbarTool.Enums;
using zwindowscore;
using zwindowscore.Enum;

namespace TaskbarTool
{
    public static class Taskbars
    {
        public static List<Taskbar> Bars { get; set; }
        public static bool MaximizedStateChanged { get; set; }
        private static string tbType;

        static Taskbars()
        {
            Bars = new List<Taskbar>();
            MaximizedStateChanged = true;
        }

        public static List<Taskbar> FindAll(bool reset = false)
        {
            if (reset)
            {
                Bars.Clear();
            }
            if(Bars.Count > 0)
            {
                return Bars;
            }

            while (true)
            {
                var hwnd = Win32Helper.FindWindow("Shell_TrayWnd", null);
                if(hwnd != IntPtr.Zero)
                {
                    Bars.Add(new Taskbar(hwnd));
                    IntPtr otherBars = IntPtr.Zero;
                    
                    while (true)
                    {
                        otherBars = Win32Helper.FindWindowEx(IntPtr.Zero, otherBars, "Shell_SecondaryTrayWnd", "");
                        if (otherBars == IntPtr.Zero) 
                        { 
                            break; 
                        }
                        else 
                        { 
                            Bars.Add(new Taskbar(otherBars)); 
                        }
                    }

                    break;
                }
            }

            return Bars;
        }

        public static void ToggleTaskbarsVisibility(bool visible, int index = 0)
        {
            FindAll(true);
            foreach(var bar in Bars)
            { 
                Win32Helper.ShowWindow(bar.HWND, visible ? CmdShow.SW_SHOW : CmdShow.SW_HIDE);
            }
        }

        public static bool IsTaskbarsVisible()
        {
            var result = false;
            FindAll(true);
            foreach(var bar in Bars)
            { 
                result = Win32Helper.IsWindowVisible(bar.HWND);
            }
            return result;
        }

        public static void ToggleTaskbarsTransparent(bool transparent, bool applyStyle = false)
        {
            var opacity = (byte)(transparent ? 0 : 255);
            var wac = WindowsAccentColor.GetColorAsInt(opacity);
            byte[] bytes = BitConverter.GetBytes(wac);
            int colorInt = BitConverter.ToInt32(new byte[] { bytes[0], bytes[1], bytes[2], opacity }, 0);

            foreach(var bar in Bars)
            { 
                bar.AccentPolicy.AccentState = AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT;
                bar.AccentPolicy.AccentFlags = 2;
                bar.AccentPolicy.GradientColor = colorInt;

                if(applyStyle)
                { 
                    ApplyStyles(bar);
                }
            }
        }

        public static void ApplyStyles()
        {
            foreach(var bar in Bars)
            {
                ApplyStyles(bar);
            }
        }
        
        public static void ApplyStyles(Taskbar taskbar)
        {
            int sizeOfPolicy = Marshal.SizeOf(taskbar.AccentPolicy);
            IntPtr policyPtr = Marshal.AllocHGlobal(sizeOfPolicy);
            Marshal.StructureToPtr(taskbar.AccentPolicy, policyPtr, false);

            WinCompatTrData data = new WinCompatTrData(WindowCompositionAttribute.WCA_ACCENT_POLICY, policyPtr, sizeOfPolicy);

            Externals.SetWindowCompositionAttribute(taskbar.HWND, ref data);

            Marshal.FreeHGlobal(policyPtr);
        }

        public static void UpdateMaximizedState()
        {
            foreach (Taskbar tb in Bars)
            {
                tb.FindMaximizedWindowsHere();
            }
            MaximizedStateChanged = false;
        }

        public static void UpdateAllSettings()
        {
            foreach (Taskbar tb in Bars)
            {
                if (tb.HasMaximizedWindow && TT.Options.Settings.UseDifferentSettingsWhenMaximized) { tbType = "Maximized"; }
                else { tbType = "Main"; }

                tb.AccentPolicy.AccentState = Globals.GetAccentState(tbType);
                tb.AccentPolicy.AccentFlags = Globals.GetAccentFlags(tbType);
                tb.AccentPolicy.GradientColor = Globals.GetTaskbarColor(tbType);
            }
        }

        public static void UpdateAccentState()
        {
            foreach (Taskbar tb in Bars)
            {
                if (tb.HasMaximizedWindow && TT.Options.Settings.UseDifferentSettingsWhenMaximized) { tbType = "Maximized"; }
                else { tbType = "Main"; }

                tb.AccentPolicy.AccentState = Globals.GetAccentState(tbType);
            }
        }

        public static void UpdateAccentFlags()
        {
            foreach (Taskbar tb in Bars)
            {
                if (tb.HasMaximizedWindow && TT.Options.Settings.UseDifferentSettingsWhenMaximized) { tbType = "Maximized"; }
                else { tbType = "Main"; }

                tb.AccentPolicy.AccentFlags = Globals.GetAccentFlags(tbType);
            }
        }

        public static void UpdateColor()
        {
            foreach (Taskbar tb in Bars)
            {
                if (tb.HasMaximizedWindow && TT.Options.Settings.UseDifferentSettingsWhenMaximized) { tbType = "Maximized"; }
                else { tbType = "Main"; }

                tb.AccentPolicy.GradientColor = Globals.GetTaskbarColor(tbType);
            }
        }
    }

    public class Taskbar
    {
        public IntPtr HWND { get; set; }
        public IntPtr Monitor { get; set; }
        public bool HasMaximizedWindow { get; set; }
        public AccentPolicy AccentPolicy;

        public Taskbar(IntPtr hwnd)
        {
            HWND = hwnd;
            Monitor = Externals.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            AccentPolicy = new AccentPolicy();


            FindMaximizedWindowsHere();
        }

        public void FindMaximizedWindowsHere()
        {
            bool isInThisScreen = false;
            IntPtr thisAppMonitor;

            foreach (IntPtr hwnd in Globals.MaximizedWindows)
            {
                thisAppMonitor = Externals.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
                if (Monitor == thisAppMonitor) { isInThisScreen = true; }
            }

            HasMaximizedWindow = isInThisScreen;
            return;
        }
    }
}