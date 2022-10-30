using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DesktopApp.HotKeys
{
    internal sealed class HotKeyWindow : NativeWindow, IDisposable
    {
        // Hot keys
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        internal event EventHandler<HotKeyPressedEventArgs> HotKeyPressed;
        internal event EventHandler<HotKeyFailedEventArgs> HotKeyFailed;

        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        internal HotKeyWindow()
        {
            
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                var pressedHotkeyCombination = new HotKeyCombination
                {
                    Modifiers = (KeyModifiers)((uint)m.LParam & 0xFFFF),
                    Key = (Keys)((uint)m.LParam >> 16),
                    Enabled = true
                };

                HotKeyPressed?.Invoke(this, new HotKeyPressedEventArgs(pressedHotkeyCombination));
            }

            base.WndProc(ref m);
        }

        public void Dispose()
        {
            DestroyHandle();
        }

        public bool RegisterHotKey(int id, uint fsmodifiers, uint vk)
        {
            _dispatcher.Invoke(new Action(() =>
            {
                var result = RegisterHotKey(Handle, id, fsmodifiers, vk);
                if (result == false)
                {
                    HotKeyFailed?.Invoke(this, new HotKeyFailedEventArgs(Marshal.GetLastWin32Error().ToString()));
                };
            }), DispatcherPriority.Background);

            return true;
        }

        public bool UnregisterHotKey(int id)
        {
            return UnregisterHotKey(Handle, id);
        }
    }
}
