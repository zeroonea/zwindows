using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zwindows.Enum
{
    public enum WinEventObjectId : int
        {
            OBJID_SELF = 0,
            OBJID_SYSMENU = -1,
            OBJID_TITLEBAR = -2,
            OBJID_MENU = -3,
            OBJID_CLIENT = -4,
            OBJID_VSCROLL = -5,
            OBJID_HSCROLL = -6,
            OBJID_SIZEGRIP = -7,
            OBJID_CARET = -8,
            OBJID_CURSOR = -9,
            OBJID_ALERT = -10,
            OBJID_SOUND = -11,
            OBJID_QUERYCLASSNAMEIDX = -12,
            OBJID_NATIVEOM = -16
        }
}
