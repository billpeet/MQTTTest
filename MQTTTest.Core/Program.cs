using System;
using System.Threading.Tasks;
using MQTTnet;

namespace MQTTTest.Core
{
    class Program
    {

        static string host = "broker.hivemq.com";
        static bool useTls = false;

        static MQTTClient client;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Attempting to connect...");

            client = new MQTTClient(host, useTls);
            client.MessageReceived += Client_MessageReceived;
            client.Connected += e => Console.WriteLine("Connected.");
            client.Disconnected += e => Console.WriteLine("Disconnected.");
            await client.SubscribeAsync("still/temperature");
            await client.ConnectAsync();

            Generate();

            Console.Read();
        }

        static void Client_MessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            // Console.WriteLine($"Message received from {e.ClientId}: {BitConverter.ToDouble(e.ApplicationMessage.Payload)}");
        }

        static async void Generate()
        {
            var random = new Random();
            var currentValue = 50.0;
            var currentTrend = 0.0;
            while (true)
            {
                currentTrend = random.Next(-2, 2);
                Console.WriteLine($"Current trend: {currentTrend}");
                currentValue += currentTrend;
                currentValue = Math.Max(10, currentValue);
                await client.PublishAsync("still/temperature", BitConverter.GetBytes(currentValue));
                await Task.Delay(500 + random.Next(-500, 500));
            }
        }

    }
}
