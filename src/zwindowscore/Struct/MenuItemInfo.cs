using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace zwindowscore.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MenuItemInfo {
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
}
