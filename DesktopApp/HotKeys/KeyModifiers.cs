using System;

namespace DesktopApp.HotKeys
{
    [Flags]
    public enum KeyModifiers
    {
        None = 0x0,
        Alt = 0x1,
        Control = 0x2,
        Shift = 0x4,
        Win = 0x8
    }
}
