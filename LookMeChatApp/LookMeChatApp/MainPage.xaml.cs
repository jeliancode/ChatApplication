using System.Collections.ObjectModel;
using System.Net.Sockets;
using Windows.ApplicationModel.Chat;

namespace LookMeChatApp;

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
        string messageContent = MessageInput.Text;
        if (!string.IsNullOrWhiteSpace(messageContent))
        {
            Message messageSent = new Message { MessageContent = messageContent, IsSentByUser = true };
            Messages.Add(messageSent);
            await _messagesManager.SendMessageAsync(messageContent);
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
