using DesktopApp.HotKeys;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace DesktopApp
{
    public class WinAppContext: ApplicationContext
    {
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private AppServiceConnection _connection;
        private HotKeyWindow _hotKeyWindow;

        public WinAppContext()
        {
            _logger.Info("WinAppContext");

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    _logger.Info("...");
                }
            });

            _hotKeyWindow = new HotKeyWindow();
            _hotKeyWindow.HotKeyFailed += _hotKeyWindow_HotKeyFailed;
            _hotKeyWindow.HotKeyPressed += _hotKeyWindow_HotKeyPressed;
            _ = StartBridge();
        }

        private async void _hotKeyWindow_HotKeyFailed(object sender, HotKeyFailedEventArgs e)
        {
            var msg = new ValueSet();
            msg.Add("failed", $"{e.Reason}");
            await _connection.SendMessageAsync(msg);
        }

        private async void _hotKeyWindow_HotKeyPressed(object sender, HotKeyPressedEventArgs e)
        {
            var msg = new ValueSet();
            msg.Add("key", $"{e.HotKeyCombination.Modifiers}+{e.HotKeyCombination.Key}");
            await _connection.SendMessageAsync(msg);
        }

        private async Task StartBridge()
        {

            _connection = new AppServiceConnection();
            var packageFamilyName = Package.Current.Id.FamilyName;

            _connection.PackageFamilyName = packageFamilyName.ToString();
            _connection.AppServiceName = "WinAppConnection";
            _connection.RequestReceived += Connection_RequestReceived;
            _connection.ServiceClosed += Connection_ServiceClosed;
            AppServiceConnectionStatus status = await _connection.OpenAsync();
        }

        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            _logger.Info("Connection_RequestReceived");

            var hotKeyCombination = new HotKeyCombination
            {
                Key = HotKeys.Keys.D,
                Modifiers = KeyModifiers.Control
            };
            _hotKeyWindow.RegisterHotKey(hotKeyCombination.GetHashCode(), (uint)hotKeyCombination.Modifiers, (uint)hotKeyCombination.Key);


            var msg = new ValueSet();
            msg.Add("key", "registered");
            await _connection.SendMessageAsync(msg);
        }

        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            _logger.Info("Connection_ServiceClosed");
        }
    }
}
