using System.Text;
using LookMeChatApp.Interface;
using LookMeChatApp.Model;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;

namespace LookMeChatApp.Services
{
    public class MessagesManager
    {
        private readonly Action<string> _messageReceivedCallback;
        private IMqttClient? _mqttClient;
        private readonly ISerializable<ChatMessage> _serializer;

        public MessagesManager(Action<string> messageReceivedCallback)
        {
            _messageReceivedCallback = messageReceivedCallback;
            _serializer = new JSONSerializable<ChatMessage>();
        }

        public async Task ConnectToMqttBrokerAsync()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("test.mosquitto.org")
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

        private Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            ChatMessage receivedMessage = _serializer.Deserialize(messageContent);
            _messageReceivedCallback?.Invoke(receivedMessage.Message);

            return Task.CompletedTask;
        }

        public async Task SendMessageAsync(ChatMessage messageSent)
        {

            string messageSerialized = _serializer.Serialize(messageSent);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/room/jesus/messages")
                .WithPayload(messageSerialized)
                .Build();

           await _mqttClient.PublishAsync(message, CancellationToken.None);
        }
    }
}
