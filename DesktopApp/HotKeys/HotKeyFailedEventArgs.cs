using System;

namespace DesktopApp.HotKeys
{
    public class HotKeyFailedEventArgs : EventArgs
    {
        public string Reason { get; private set; }

        public HotKeyFailedEventArgs(string reason)
        {
            Reason = reason; ;
        }
    }
}
