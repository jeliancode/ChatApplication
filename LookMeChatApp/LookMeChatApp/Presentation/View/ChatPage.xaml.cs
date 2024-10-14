using System.Collections.ObjectModel;
using LookMeChatApp.ApplicationLayer.ViewModel;
using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Domain.Model;
using Microsoft.UI.Xaml.Input;

namespace LookMeChatApp.Presentation.View;

public sealed partial class ChatPage : Page
{
    private readonly ChatViewModel _chatViewModel;

    public ChatPage()
    {
        this.InitializeComponent();
        var connectionManager = new ConnectionHandler<ChatMessage>(); ;
        var messageRepository = App.SQLiteDb.MessageRepository;

        var connectClientUseCase = new ConnectClientUseCase<ChatMessage>(connectionManager);
        var subscribToTopicUseCase = new SubscribToTopicUseCase<ChatMessage>(connectionManager);
        var sendMessageUseCase = new SendMessageUseCase<ChatMessage>(connectionManager);

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

    private void SenderId_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (sender is TextBlock textBlock && textBlock.DataContext is ChatMessage chatMessage)
        {
            _chatViewModel.CaptureSenderId(chatMessage.SenderId);
        }
    }
}
