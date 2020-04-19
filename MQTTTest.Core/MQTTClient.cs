using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTTest.Core
{
    public class MQTTClient
    {
        public IManagedMqttClient Client { get; }

        public string ClientId { get; set; }
        public string TcpServer { get; set; }
        public bool UseTls { get; set; }

        public delegate void MessageReceivedEvent(MqttApplicationMessageReceivedEventArgs e);
        public delegate void ConnectedEvent(MqttClientConnectedEventArgs e);
        public delegate void DisconnectedEvent(MqttClientDisconnectedEventArgs e);

        public event MessageReceivedEvent MessageReceived;
        public event ConnectedEvent Connected;
        public event DisconnectedEvent Disconnected;

        public MQTTClient(string tcpServer, bool useTls = false, string clientId = null)
        {
            Client = new MqttFactory().CreateManagedMqttClient();
            Client.UseApplicationMessageReceivedHandler(e => MessageReceived?.Invoke(e));
            Client.UseConnectedHandler(e => Connected?.Invoke(e));
            Client.UseDisconnectedHandler(e => Disconnected?.Invoke(e));
            ClientId = clientId ?? Guid.NewGuid().ToString();
            TcpServer = tcpServer;
            UseTls = useTls;
        }

        public async Task ConnectAsync()
        {
            var clientOptions = new MqttClientOptionsBuilder()
                .WithClientId(ClientId)
                .WithTcpServer(TcpServer);
            if (UseTls)
                clientOptions.WithTls();

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(clientOptions.Build())
                .Build();
            await Client.StartAsync(options);
        }

        public async Task SubscribeAsync(string topic)
        {
            await Client.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());
        }

        public async Task PublishAsync(string topic, byte[] payload)
        {
            await Client.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build());
        }

    }
}