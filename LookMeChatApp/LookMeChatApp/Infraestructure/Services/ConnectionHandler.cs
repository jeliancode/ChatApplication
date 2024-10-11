using System.Text;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;

namespace LookMeChatApp.Infraestructure.Services
{
    public class ConnectionHandler : IConnectionHandler
    {
        public event Action<ChatMessage> MessageReceived;
        private IMqttClient? _mqttClient;
        private MqttFactory _mqttFactory;
        private MqttClientOptions _options;
        private readonly ISerializable<ChatMessage> _serializer;
        private string server ;

        public ConnectionHandler()
        {
            _serializer = new JSONSerializable<ChatMessage>();
            server = "test.mosquitto.org";
        }

        public async Task ConnectToMqttBrokerAsync()
        {
            _mqttFactory = new MqttFactory(); //Connection
            _mqttClient = _mqttFactory.CreateMqttClient(); //Connection (mqqTClien)

            _options = new MqttClientOptionsBuilder() //Connection
                .WithTcpServer(server)
                .Build();
        }

        public async Task SubscribeToTopicAsync()
        {
            _mqttClient.ConnectedAsync += e =>
            {
                var topic = new MqttTopicFilterBuilder()
                .WithTopic("/v1/room/+/messages")
                .Build();

            _mqttClient.SubscribeAsync(topic);
            return Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageReceivedAsync += ReceiveMessageAsync;

            await _mqttClient.ConnectAsync(_options, CancellationToken.None);
        }

        public async Task SendMessageAsync(ChatMessage messageSent)
        {
            _mqttClient.ConnectAsync(_options, CancellationToken.None);
            string messageSerialized = _serializer.Serialize(messageSent);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/v1/room/jesus/messages")
                .WithPayload(messageSerialized)
                .Build();

           await _mqttClient.PublishAsync(message, CancellationToken.None);
        }

        public Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            ChatMessage receivedMessage = _serializer.Deserialize(messageContent);
            MessageReceived?.Invoke(receivedMessage);

            return Task.CompletedTask;
        }
    }
}
