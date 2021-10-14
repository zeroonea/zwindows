using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zwindows.Enum
{
    public enum EventType : uint
        {
            EVENT_MIN = 0x00000001,
            EVENT_MAX = 0x7FFFFFFF,
            EVENT_OBJECT_LOCATIONCHANGE = 0x800B,

            EVENT_SYSTEM_MINIMIZESTART = 0x0016,
            EVENT_SYSTEM_MINIMIZEEND = 0x0017,

            EVENT_SYSTEM_FOREGROUND = 0x0003,

            EVENT_OBJECT_INVOKED = 0x8013,

            EVENT_SYSTEM_DESKTOPSWITCH = 0x0020,

            EVENT_SYSTEM_MOVESIZESTART = 0x000A,

            EVENT_SYSTEM_MOVESIZEEND = 0x000B,

            EVENT_OBJECT_STATECHANGE = 0x800A,

            EVENT_OBJECT_NAMECHANGE = 0x800C,

            EVENT_OBJECT_DESTROY = 0x8001,

            EVENT_OBJECT_FOCUS = 0x8005,

            EVENT_SYSTEM_CAPTURESTART = 0x0008,

            EVENT_SYSTEM_CAPTUREEND = 0x0009,

            EVENT_OBJECT_REORDER = 0x8004
        }
}
