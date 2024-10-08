using System.Collections.ObjectModel;
using System.Net.Sockets;
using LookMeChatApp.Model;
using Windows.ApplicationModel.Chat;
using LookMeChatApp.Service;

namespace LookMeChatApp.View;

public sealed partial class MainPage : Page
{
    public ObservableCollection<Message> Messages { get; set; }
    private MessagesManager _messagesManager;

    public MainPage()
    {
        this.InitializeComponent();
        Messages = new ObservableCollection<Message>();
        MessagesList.ItemsSource = Messages;
        _messagesManager = new MessagesManager(OnMessageReceived);

        _messagesManager.ConnectToMqttBrokerAsync();
    }

    private async void SendMessage(object sender, RoutedEventArgs e)
    {
        string message = MessageInput.Text;
        if (!string.IsNullOrWhiteSpace(message))
        {
            Message messageSent = new Message 
            { 
                MessageId = "76707551",
                MessageContent = message,
                Username = "Jesus",
                Room = "room",
                IsSentByUser = true
            };

            Messages.Add(messageSent);

            await _messagesManager.SendMessageAsync(messageSent);
            MessageInput.Text = "";
        }
    }

    private void OnMessageReceived(string message)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            Message messageReceived = new Message { MessageContent = message, IsSentByUser = false };
            Messages.Add(messageReceived);
        });
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
    }
}
