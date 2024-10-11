using LookMeChatApp.Domain.Model;
using MQTTnet.Client;

namespace LookMeChatApp.Domain.Interface;

public interface IConnectionHandler
{
    event Action<ChatMessage> MessageReceived;
    Task ConnectToMqttBrokerAsync();
    Task SubscribeToTopicAsync();
    Task SendMessageAsync(ChatMessage message);
    Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e);
}
