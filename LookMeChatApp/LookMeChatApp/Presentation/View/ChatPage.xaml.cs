using System.Collections.ObjectModel;
using LookMeChatApp.ApplicationLayer.ViewModel;
using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Presentation.View;

public sealed partial class ChatPage : Page
{
    private readonly ChatViewModel _chatViewModel;

    public ChatPage()
    {
        this.InitializeComponent();
        var connectionManager = App.ConnectionHandler;
        var messageRepository = App.SQLiteDb.MessageRepository;

        var connectClientUseCase = new ConnectClientUseCase(connectionManager);
        var subscribToTopicUseCase = new SubscribToTopicUseCase(connectionManager);
        var sendMessageUseCase = new SendMessageUseCase(connectionManager);

        _chatViewModel = new ChatViewModel(sendMessageUseCase, connectClientUseCase, messageRepository, subscribToTopicUseCase);
        this.DataContext = _chatViewModel;

        connectionManager.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(ChatMessage message)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            _chatViewModel.OnMessageReceived(message);
        });
    }
}
