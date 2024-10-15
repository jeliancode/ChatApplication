using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Infraestructure.Repositories;
using LookMeChatApp.Infraestructure.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class ChatViewModel : INotifyPropertyChanged
{
    public string MessageInput { get; set; }
    private ObservableCollection<ChatMessage> messages;
    private readonly ConnectClientUseCase<ChatMessage> _connectClientUseCase;
    private readonly SubscribToTopicUseCase<ChatMessage> _subscribeToTopicUseCase;
    private readonly SendMessageUseCase<ChatMessage> _sendMessageUseCase;
    private readonly AccountSessionService accountSessionService;
    private readonly TopicSessionService topicSessionService;
    private readonly AddContactService addContactService;
    private readonly SQLiteDb sQLiteDb;
    private readonly AESCryptoService aesCryptoService;
    private readonly INavigation navigation;
    private IMessageRepository _messageRepository;
    public ICommand SendMessageCommand { get; }
    public ICommand ComeBackCommand { get; }


    public event PropertyChangedEventHandler? PropertyChanged;

    public ChatViewModel(SendMessageUseCase<ChatMessage> sendMessageUseCase, ConnectClientUseCase<ChatMessage> connectClientUseCase, IMessageRepository messageRepository, SubscribToTopicUseCase<ChatMessage> subscribToTopicUseCase)
    {
        _connectClientUseCase = connectClientUseCase;
        _sendMessageUseCase = sendMessageUseCase;
        _subscribeToTopicUseCase = subscribToTopicUseCase;
        _messageRepository = messageRepository;

        navigation = App.NavigationService;
        sQLiteDb = App.SQLiteDb;
        accountSessionService = new AccountSessionService();
        topicSessionService = new TopicSessionService();
        addContactService = new AddContactService();
        aesCryptoService = new AESCryptoService();
        messages = new ObservableCollection<ChatMessage>();

        SendMessageCommand = new RelayCommand(async () => await SendMessage());
        ComeBackCommand = new RelayCommand(ComeBack);

        _connectClientUseCase.ExecuteAsync();
        _subscribeToTopicUseCase.ExecuteAsync();
        LoadMessagesAsync();
    }

    public ObservableCollection<ChatMessage> Messages
    {
        get => messages;
        set
        {
            messages = value;
            OnPropertyChanged(nameof(Messages));
        }
    }

    private async Task LoadMessagesAsync()
    {
        var roomName = topicSessionService.GetCurrentRoomName();
        var version = topicSessionService.GetCurrentVersion();
        var room = $"/{version}/room/+/{roomName}";

        var messagesRepository = sQLiteDb.MessageRepository;
        var messagesList = await messagesRepository.GetMessagesByRoom(room);

        Messages.Clear();

        foreach (var message in messagesList)
        {
            Messages.Add(message);

        }
    }

    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(MessageInput))
        {
            var userId = accountSessionService.GetCurrentUserId();
            var encryptedMessage = aesCryptoService.EncryptMessage(MessageInput);
            var user = accountSessionService.GetCurrentUsername();
            var version = topicSessionService.GetCurrentVersion();
            var roomName = topicSessionService.GetCurrentRoomName();
            var room = $"/{version}/room/+/{roomName}/";

            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Message = Convert.ToBase64String(encryptedMessage), //quitar encriptacion
                SenderId = userId,
                Room = room,
                Timestamp = DateTime.UtcNow.ToString(),
            };


            var currentUserPath = $"/{version}/room/{user}/{roomName}";

            MessageInput = string.Empty;
            OnPropertyChanged(nameof(MessageInput));

            await _sendMessageUseCase.ExecuteAsync(message, currentUserPath);
        }
    }

    public void CaptureSenderId(Guid senderId)
    {
        if (senderId != Guid.Empty)
        {
            addContactService.SetCurrentContactId(senderId);
            navigation.NavigateTo("Contact");
        }
    }

    private void ComeBack()
    {
        topicSessionService.ClearCurrentVersion();
        navigation.ComeBack();
    }

    public async void OnMessageReceived(ChatMessage receivedMessage)
    {
        await SaveMessage(receivedMessage);
        Messages.Add(receivedMessage);
    }


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task SaveMessage(ChatMessage message)
    {
        await _messageRepository.AddMessageAsync(message);
    }
}
