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
        var messagesManager = new MessagesManager();
        var sendMessageUseCase = new SendMessageUseCase(messagesManager);
        var receiveMessageUseCase = new ReceiveMessageUseCase(messagesManager);
        var messageRepository = App.SQLiteDb.MessageRepository;

        _chatViewModel = new ChatViewModel(sendMessageUseCase, receiveMessageUseCase, messageRepository);
        this.DataContext = _chatViewModel;

        messagesManager.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(ChatMessage message)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            _chatViewModel.OnMessageReceived(message);
        });
    }
}
