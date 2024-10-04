using System.Text;
using LookMeChatApp.Interface;
using LookMeChatApp.Model;
using LookMeChatApp.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;

namespace LookMeChatApp.Service
{
    public class MessagesManager
    {
        private readonly Action<string> _messageReceivedCallback;
        private IMqttClient? _mqttClient;
        private readonly ISerializable<Message> _serializer;

        public MessagesManager(Action<string> messageReceivedCallback)
        {
            _messageReceivedCallback = messageReceivedCallback;
            _serializer = new JSONSerializable<Message>();
        }

        public async Task ConnectToMqttBrokerAsync()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("test.mosquitto.org")
                .Build();

            //Bug 
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
               string messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                Message? receivedMessage = _serializer.Deserialize(messageContent);
               _messageReceivedCallback?.Invoke(messageContent);
               return Task.CompletedTask;
            };

            await _mqttClient.ConnectAsync(options, CancellationToken.None);

            var topic = new MqttTopicFilterBuilder()
                .WithTopic("/room/+/messages")
                .Build();

            await _mqttClient.SubscribeAsync(topic);
        }


        public async Task SendMessageAsync(Message messageSent)
        {

            string messageSerialized = _serializer.Serialize(messageSent);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/room/jesus/messages")
                .WithPayload(messageSerialized)
                .Build();

            if (_mqttClient.IsConnected)
            {
                await _mqttClient.PublishAsync(message, CancellationToken.None);
            }
        }
    }
}
