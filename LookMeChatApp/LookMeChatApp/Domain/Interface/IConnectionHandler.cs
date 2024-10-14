using LookMeChatApp.Domain.Model;
using MQTTnet.Client;

namespace LookMeChatApp.Domain.Interface;

public interface IConnectionHandler <A>
{
    event Action<A> MessageReceived;
    Task ConnectToMqttBrokerAsync();
    Task SubscribeToTopicAsync();
    Task SendMessageAsync(A message, string topicPath);
    Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e);
}
