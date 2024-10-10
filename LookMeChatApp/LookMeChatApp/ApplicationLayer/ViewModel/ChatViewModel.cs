using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Infraestructure.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class ChatViewModel : INotifyPropertyChanged
{
    public string MessageInput { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly SendMessageUseCase _sendMessageUseCase;
    private readonly ReceiveMessageUseCase _receiveMessageUseCase;
    private IMessageRepository _messageRepository;
    public ICommand SendMessageCommand { get; }
    private readonly AccountSessionService _accountSessionService;
    private ObservableCollection<ChatMessage> _messages;
    public ObservableCollection<ChatMessage> Messages 
    {
        get => _messages;
        set
        {
            _messages = value;
            OnPropertyChanged(nameof(Messages));
        }
    }


    public ChatViewModel(SendMessageUseCase sendMessageUseCase, ReceiveMessageUseCase receiveMessageUseCase, IMessageRepository messageRepository)
    {       
        _sendMessageUseCase = sendMessageUseCase;
        _receiveMessageUseCase = receiveMessageUseCase;
        _accountSessionService = new AccountSessionService();
        _messages = new ObservableCollection<ChatMessage>();
        SendMessageCommand = new RelayCommand(async () => await SendMessage());
        _receiveMessageUseCase.ExecuteAsync();
        _messageRepository = messageRepository;
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
                Room = "room",
                Timestamp = DateTime.UtcNow.ToString(),
            };



            MessageInput = string.Empty;
            OnPropertyChanged(nameof(MessageInput));

            await _sendMessageUseCase.ExecuteAsync(message);  
        }
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
