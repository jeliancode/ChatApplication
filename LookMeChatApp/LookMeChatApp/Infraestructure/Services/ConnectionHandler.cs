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
    private IMqttClient? _mqttClient;
    private MqttFactory _mqttFactory;
    private MqttClientOptions _options;
    private readonly TopicSessionService _topicSessionService;
    private readonly AccountSessionService _accountSessionService;
    private string server;
    private string currentUserPath;
    private string topicToSubscribe;

    public ConnectionHandler()
    { 
        serializer = new JSONSerializable<A>();
        _topicSessionService = new TopicSessionService();
        _accountSessionService = new AccountSessionService();

        var version = _topicSessionService.GetCurrentVersion();
        var room = _topicSessionService.GetCurrentRoomName();
        var user = _accountSessionService.GetCurrentUsername();
        topicToSubscribe = $"/{version}/room/+/{room}";
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

    public async Task SendMessageAsync(A messageSent, string topicPath)
    {
        _mqttClient.ConnectAsync(_options, CancellationToken.None);
        var messageSerialized = serializer.Serialize(messageSent);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topicPath)
            .WithPayload(messageSerialized)
            .Build();

       await _mqttClient.PublishAsync(message, CancellationToken.None);
    }

    public Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var messageContent = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        var receivedMessage = serializer.Deserialize(messageContent);
        MessageReceived?.Invoke(receivedMessage);

        return Task.CompletedTask;
    }
}
