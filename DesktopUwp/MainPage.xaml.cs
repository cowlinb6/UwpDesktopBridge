using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http.Headers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DesktopUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            App.AppServiceConnected += App_AppServiceConnected; ;
            Task.Run(async () => await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync());
        }

        private void App_AppServiceConnected(object sender, EventArgs e)
        {
            App.Connection.RequestReceived += Connection_RequestReceived;
        }

        private async void Connection_RequestReceived(Windows.ApplicationModel.AppService.AppServiceConnection sender, Windows.ApplicationModel.AppService.AppServiceRequestReceivedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                foreach (var a in args.Request.Message)
                {
                    txtLog.Text = $"{DateTime.Now} {a.Key}={a.Value}\n{txtLog.Text}";
                }
            });
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            var data = new ValueSet();
            data.Add("key", "val");

            await App.Connection.SendMessageAsync(data);
        }
    }
}
