using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTTest.Core;

namespace MQTTTest.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ChartViewModel Chart { get; }

        public MQTTClient MQTTClient { get; }

        public MainViewModel()
        {
            Chart = new ChartViewModel();
            MQTTClient = new MQTTClient("broker.hivemq.com");
            MQTTClient.MessageReceived += MQTTClient_MessageReceived;
            SetupMQTTClient();
        }

        private async void SetupMQTTClient()
        {
            await MQTTClient.SubscribeAsync("still/temperature");
            await MQTTClient.ConnectAsync();
        }

        private void MQTTClient_MessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var res = BitConverter.ToDouble(e.ApplicationMessage.Payload);
            Chart.AddDataPoint(res);
        }

        private async void StartGenerating()
        {
            var random = new Random();
            var currentValue = 50.0;
            var currentTrend = 0.1;
            while (true)
            {
                currentTrend += random.Next(-2, 2) / 10;
                currentValue += currentTrend;
                Chart.AddDataPoint(new DataViewModel(DateTime.Now, currentValue));
                await Task.Delay(500 + random.Next(-500, 500));
            }
        }

    }
}