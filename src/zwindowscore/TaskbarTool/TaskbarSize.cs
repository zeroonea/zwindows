using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using zwindowscore;
using zwindowscore.Struct;

namespace TaskbarTool
{
    public sealed class TaskbarSize
    {
        private const string ClassName = "Shell_TrayWnd";

        public System.Drawing.Rectangle Bounds
        {
            get;
            private set;
        }
        public TaskbarPosition Position
        {
            get;
            private set;
        }
        public System.Drawing.Point Location
        {
            get
            {
                return this.Bounds.Location;
            }
        }
        public System.Drawing.Size Size
        {
            get
            {
                return this.Bounds.Size;
            }
        }
        //Always returns false under Windows 7
        public bool AlwaysOnTop
        {
            get;
            private set;
        }
        public bool AutoHide
        {
            get;
            private set;
        }

        public TaskbarSize()
        {
            IntPtr taskbarHandle = Win32Helper.FindWindow(TaskbarSize.ClassName, null);

            APPBARDATA data = new APPBARDATA();
            data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
            data.hWnd = taskbarHandle;
            IntPtr result = SHAppBarMessage(ABM.GetTaskbarPos, ref data);
            if (result == IntPtr.Zero)
                throw new InvalidOperationException();

            this.Position = (TaskbarPosition)data.uEdge;
            this.Bounds = System.Drawing.Rectangle.FromLTRB(data.rc.Left, data.rc.Top, data.rc.Right, data.rc.Bottom);

            data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
            result = SHAppBarMessage(ABM.GetState, ref data);
            int state = result.ToInt32();
            this.AlwaysOnTop = (state & ABS.AlwaysOnTop) == ABS.AlwaysOnTop;
            this.AutoHide = (state & ABS.Autohide) == ABS.Autohide;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);
    }

    public enum TaskbarPosition
    {
        Unknown = -1,
        Left,
        Top,
        Right,
        Bottom,
    }

    public enum ABM : uint
    {
        New = 0x00000000,
        Remove = 0x00000001,
        QueryPos = 0x00000002,
        SetPos = 0x00000003,
        GetState = 0x00000004,
        GetTaskbarPos = 0x00000005,
        Activate = 0x00000006,
        GetAutoHideBar = 0x00000007,
        SetAutoHideBar = 0x00000008,
        WindowPosChanged = 0x00000009,
        SetState = 0x0000000A,
    }

    public enum ABE : uint
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    public static class ABS
    {
        public const int Autohide = 0x0000001;
        public const int AlwaysOnTop = 0x0000002;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public ABE uEdge;
        public Rect rc;
        public int lParam;
    }
}
