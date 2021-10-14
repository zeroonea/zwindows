using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using zwindows.Struct;

namespace zwindows
{
    public static class MouseHook
    {
        public static event EventHandler MouseAction = delegate { };

        public static void Start()
        {
            _hookID = SetHook(_proc);


        }
        public static void Stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
             return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle("user32"), 0);
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var mm = (MouseMessages)wParam;
            var cancel = false;

            if(nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == mm)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                IntPtr handle = GetForegroundWindow();

                var menu = Win32Helper.GetSystemMenu(handle, false);
                var n = Win32Helper.GetMenuItemCount(menu);

                Console.WriteLine("{0:x8} {1:x8} menu count: {2}", handle.ToInt32(), menu.ToInt32(), n);

                /*var hwnd = Win32Helper.WindowFromPoint(hookStruct.pt);
                var hwnd2 = Win32Helper.ChildWindowFromPoint(hwnd, new Point{ X = 1, Y = 1 });
                var hwnd3 = Win32Helper.RealChildWindowFromPoint(hwnd, new Point{ X = 1, Y = 1 });

                var sb = new StringBuilder(100);

                Win32Helper.GetClassName(hwnd, sb, sb.Capacity);
                var sb1 = new StringBuilder(100);
                Win32Helper.GetClassName(hwnd, sb1, sb1.Capacity);
                var sb2 = new StringBuilder(100);
                Win32Helper.GetClassName(hwnd, sb2, sb2.Capacity);

                Console.WriteLine("up {0:x8} {1:x8} {2:x8}", hwnd.ToInt32(), 
                    hwnd2.ToInt32(),
                    hwnd3.ToInt32());
                Console.WriteLine(sb.ToString());
                Console.WriteLine(sb1.ToString());
                Console.WriteLine(sb2.ToString());

                if(hwnd.ToInt32().ToString("x8") == "00f416d2")
                {
                    cancel = true;
                }*/

                //Console.WriteLine("lup {0} {1} {2}", nCode, hookStruct.mouseData, hookStruct.dwExtraInfo);
            }

            /*if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == mm)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(null, new EventArgs());
                
                IntPtr handle = GetForegroundWindow();
                Console.WriteLine("handle: {0:x8}", handle.ToInt32());

                Console.WriteLine("{0} {1:x8}", hookStruct.mouseData, hookStruct.dwExtraInfo.ToInt32());
            }*/
            var next = CallNextHookEx(_hookID, nCode, wParam, lParam);

            if(cancel)
            {
                return new IntPtr(1);
            }
            return next;
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public Point pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetActiveWindow();
    }
}