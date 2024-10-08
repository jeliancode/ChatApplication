using System.Collections.ObjectModel;
using LookMeChatApp.Model;
using LookMeChatApp.Services;

namespace LookMeChatApp.View;

public sealed partial class MainPage : Page
{
    public ObservableCollection<ChatMessage> Messages { get; set; }
    private MessagesManager _messagesManager;

    public MainPage()
    {
        this.InitializeComponent();
        Messages = new ObservableCollection<ChatMessage>();
        MessagesList.ItemsSource = Messages;
        _messagesManager = new MessagesManager(OnMessageReceived);

        _messagesManager.ConnectToMqttBrokerAsync();
    }

    private async void SendMessage(object sender, RoutedEventArgs e)
    {
        string message = MessageInput.Text;
        if (!string.IsNullOrWhiteSpace(message))
        {
            ChatMessage messageSent = new ChatMessage 
            { 
                
                Message = message,
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
            ChatMessage messageReceived = new ChatMessage { Message = message, IsSentByUser = false };
            Messages.Add(messageReceived);
        });
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
    }
}
