using System.Text;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;

namespace LookMeChatApp.Infraestructure.Services
{
    public class MessagesManager : IMessageHandler
    {
        public event Action<ChatMessage> MessageReceived;
        private IMqttClient? _mqttClient;
        private readonly ISerializable<ChatMessage> _serializer;
        private string server ;


        public MessagesManager()
        {
            _serializer = new JSONSerializable<ChatMessage>();
            server = "test.mosquitto.org";
        }

        public async Task ConnectToMqttBrokerAsync()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(server)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += ReceiveMessageAsync;
            await _mqttClient.ConnectAsync(options, CancellationToken.None);
            await SubscribeToTopicAsync();
        }

        private async Task SubscribeToTopicAsync()
        {
            var topic = new MqttTopicFilterBuilder()
                .WithTopic("/room/+/messages")
                .Build();

            await _mqttClient.SubscribeAsync(topic);
        }

        public async Task SendMessageAsync(ChatMessage messageSent)
        {

            string messageSerialized = _serializer.Serialize(messageSent); // AJUSTAR PARA VERSIONES DE PATH

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/room/jesus/messages")
                .WithPayload(messageSerialized)
                .Build();

           await _mqttClient.PublishAsync(message, CancellationToken.None);
        }

        public Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            ChatMessage receivedMessage = _serializer.Deserialize(messageContent); //AJUSTAR PARA VERSIONNES DE PATH
            MessageReceived?.Invoke(receivedMessage);

            return Task.CompletedTask;
        }
    }
}
