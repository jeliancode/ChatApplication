using System.Text;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;

namespace LookMeChatApp.Infraestructure.Services;

public class ConnectionHandler<A> : IConnectionHandler<A>
{
    public event Action<A> MessageReceived;
    private readonly ISerializable<A> serializer;
    private IMqttClient? mqttClient;
    private MqttFactory mqttFactory;
    private MqttClientOptions _options;
    private readonly TopicSessionService topicSessionService;
    private readonly AccountSessionService accountSessionService;

    private readonly string server;
    private readonly string topicToSubscribe;

    public ConnectionHandler()
    { 
        serializer = new JSONSerializable<A>();
        topicSessionService = new TopicSessionService();
        accountSessionService = new AccountSessionService();

        var version = topicSessionService.GetCurrentVersion();
        var room = topicSessionService.GetCurrentRoomName();
        var user = accountSessionService.GetCurrentUsername();
        topicToSubscribe = $"/{version}/room/+/{room}";
        server = "146.190.213.152";
    }

    public async Task ConnectToMqttBrokerAsync()
    {
        mqttFactory = new MqttFactory();
        mqttClient = mqttFactory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer(server)
            .Build();
    }

    public async Task SubscribeToTopicAsync()
    {
        mqttClient.ConnectedAsync += e =>
        {
            var topic = new MqttTopicFilterBuilder()
            .WithTopic(topicToSubscribe)
            .Build();

        mqttClient.SubscribeAsync(topic);
        return Task.CompletedTask;
        };

        mqttClient.ApplicationMessageReceivedAsync += ReceiveMessageAsync;

        await mqttClient.ConnectAsync(_options, CancellationToken.None);
    }

    public async Task SendMessageAsync(A messageSent, string topicPath)
    {
        if (!mqttClient.IsConnected)
        {
            await mqttClient.ConnectAsync(_options, CancellationToken.None);
        }
        var messageSerialized = serializer.Serialize(messageSent);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topicPath)
            .WithPayload(messageSerialized)
            .Build();

       await mqttClient.PublishAsync(message, CancellationToken.None);
    }

    public Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        var receivedMessage = serializer.Deserialize(messageContent);
        MessageReceived?.Invoke(receivedMessage);

        return Task.CompletedTask;
    }
}
