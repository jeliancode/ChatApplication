using LookMeChatApp.ApplicationLayer.Use_Cases;
using LookMeChatApp.Domain.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace LookMeChatApp.ApplicationLayer.ViewModel;

public class ChatViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly SendMessageUseCase _sendMessageUseCase;
    private readonly ReceiveMessageUseCase _receiveMessageUseCase;

    public ObservableCollection<ChatMessage> Messages { get; set; }
    public ICommand SendMessageCommand { get; }
    public string MessageInput { get; set; }

    public ChatViewModel(SendMessageUseCase sendMessageUseCase, ReceiveMessageUseCase receiveMessageUseCase)
    {
        
        _sendMessageUseCase = sendMessageUseCase;
        _receiveMessageUseCase = receiveMessageUseCase;
        Messages = new ObservableCollection<ChatMessage>();

        SendMessageCommand = new RelayCommand(async () => await SendMessage());
        _receiveMessageUseCase.ExecuteAsync();
    }

    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(MessageInput))
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Message = MessageInput,
                SenderId = Guid.NewGuid(),
                Room = "room",
                Timestamp = DateTime.UtcNow.ToString(),
                IsSentByUser = true
            };

            Messages.Add(message);
            MessageInput = string.Empty;
            await _sendMessageUseCase.ExecuteAsync(message);
            OnPropertyChanged(nameof(MessageInput));
        }
    }

    public void OnMessageReceived(string messageContent)
    {
        var receivedMessage = new ChatMessage
        {
            Message = messageContent,
            IsSentByUser = false
        };

        Messages.Add(receivedMessage);
    }


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

