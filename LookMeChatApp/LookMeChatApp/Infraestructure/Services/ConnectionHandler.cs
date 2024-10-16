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
        private readonly ISerializable<ChatMessage> _serializer;
        private IMqttClient? _mqttClient;
        private MqttFactory _mqttFactory;
        private MqttClientOptions _options;
        private readonly TopicSessionService _topicSessionService;
        private string server;
        private string currentUserPath;
        private string topicToSubscribe;

        public ConnectionHandler()
        {
            _serializer = new JSONSerializable<ChatMessage>();
            _topicSessionService = new TopicSessionService();

            topicToSubscribe = _topicSessionService.GetCurrentTopic();
            currentUserPath = _topicSessionService.GetCurrentUserPath();
            server = "test.mosquitto.org";
        }

        public async Task ConnectToMqttBrokerAsync()
        {
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(server)
                .Build();
        }

        public async Task SubscribeToTopicAsync()
        {
            _mqttClient.ConnectedAsync += e =>
            {
                var topic = new MqttTopicFilterBuilder()
                .WithTopic(topicToSubscribe)
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
                .WithTopic(currentUserPath)
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
