using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;

namespace LookMeChatApp
{
    public class MessagesManager
    {
        private readonly Action<string> _messageReceivedCallback;
        private IMqttClient? _mqttClient;

        public MessagesManager(Action<string> messageReceivedCallback)
        {
            _messageReceivedCallback = messageReceivedCallback;

        }

        public async Task ConnectToMqttBrokerAsync()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("test.mosquitto.org")
                .Build();

            _mqttClient.ConnectedAsync += e =>
            {
                var topic = new MqttTopicFilterBuilder()
                .WithTopic("/room/+/messages")
                .Build();
                _mqttClient.SubscribeAsync(topic);

                return Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
               string messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
               _messageReceivedCallback?.Invoke(messageContent);
               return Task.CompletedTask;
            };
            
            await _mqttClient.ConnectAsync(options, CancellationToken.None);

        }


        public async Task SendMessageAsync(string messageContent)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/room/jesus/messages")
                .WithPayload(messageContent)
                .Build();

            if (_mqttClient.IsConnected)
            {
                await _mqttClient.PublishAsync(message, CancellationToken.None);
            }
        }
    }
}
