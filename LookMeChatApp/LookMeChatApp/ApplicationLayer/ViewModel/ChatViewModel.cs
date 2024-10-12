using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Infraestructure.Repositories;
using LookMeChatApp.Infraestructure.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class ChatViewModel : INotifyPropertyChanged
{
    public string MessageInput { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly ConnectClientUseCase _connectClientUseCase;
    private readonly SubscribToTopicUseCase _subscribeToTopicUseCase;
    private readonly SendMessageUseCase _sendMessageUseCase;
    private readonly AccountSessionService _accountSessionService;
    private readonly TopicSessionService _topicSessionService;
    private readonly SQLiteDb sQLiteDb;
    private readonly INavigation navigation;
    private IMessageRepository _messageRepository;
    public ICommand SendMessageCommand { get; }
    public ICommand ComeBackCommand { get; }
    
    private ObservableCollection<ChatMessage> _messages;

    public ChatViewModel(SendMessageUseCase sendMessageUseCase, ConnectClientUseCase connectClientUseCase, IMessageRepository messageRepository, SubscribToTopicUseCase subscribToTopicUseCase)
    {
        _connectClientUseCase = connectClientUseCase;
        _sendMessageUseCase = sendMessageUseCase;
        _subscribeToTopicUseCase = subscribToTopicUseCase;
        _messageRepository = messageRepository;
        _connectClientUseCase.ExecuteAsync();

        navigation = App.NavigationService;
        sQLiteDb = App.SQLiteDb;
        _accountSessionService = new AccountSessionService();
        _topicSessionService = new TopicSessionService();
        _messages = new ObservableCollection<ChatMessage>();

        SendMessageCommand = new RelayCommand(async () => await SendMessage());
        ComeBackCommand = new RelayCommand(ComeBack);

        _connectClientUseCase.ExecuteAsync();
        _subscribeToTopicUseCase.ExecuteAsync();

        LoadMessagesAsync();
    }

    public ObservableCollection<ChatMessage> Messages
    {
        get => _messages;
        set
        {
            _messages = value;
            OnPropertyChanged(nameof(Messages));
        }
    }

    private async Task LoadMessagesAsync()
    {
        var room = _topicSessionService.GetCurrentRoomName();
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
            Guid userId = _accountSessionService.GetCurrentUserId();
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Message = MessageInput,
                SenderId = userId,
                Room = _topicSessionService.GetCurrentRoomName(),
                Timestamp = DateTime.UtcNow.ToString(),
            };



            MessageInput = string.Empty;
            OnPropertyChanged(nameof(MessageInput));

            await _sendMessageUseCase.ExecuteAsync(message);  
        }
    }

    private void ComeBack()
    {
        _topicSessionService.ClearCurrentVersion();
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
