using System;

namespace DesktopApp.HotKeys
{
    public class HotKeyPressedEventArgs : EventArgs
    {
        public HotKeyCombination HotKeyCombination { get; private set; }

        public HotKeyPressedEventArgs(HotKeyCombination hotKeyCombination)
        {
            HotKeyCombination = hotKeyCombination;
        }
    }
}
