using System.Collections.ObjectModel;
using LookMeChatApp.ApplicationLayer.ViewModel;
using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Infraestructure.Services;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Presentation.View;

public sealed partial class MainPage : Page
{
    private readonly ChatViewModel _chatViewModel;

    public MainPage()
    {
        this.InitializeComponent();
        var messagesManager = new MessagesManager(OnMessageReceived);
        var sendMessageUseCase = new SendMessageUseCase(messagesManager);
        var receiveMessageUseCase = new ReceiveMessageUseCase(messagesManager);

        _chatViewModel = new ChatViewModel(sendMessageUseCase, receiveMessageUseCase);
        this.DataContext = _chatViewModel;
    }

    private void OnMessageReceived(string message)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            _chatViewModel.OnMessageReceived(message);
        });
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
    }
}
