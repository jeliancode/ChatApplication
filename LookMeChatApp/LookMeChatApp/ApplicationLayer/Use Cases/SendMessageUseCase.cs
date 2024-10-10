using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.ApplicationLayer.Use_Cases;

public class SendMessageUseCase
{
    private readonly IMessageHandler _messageHandler;


    public SendMessageUseCase(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task ExecuteAsync(ChatMessage message)
    {
        await _messageHandler.SendMessageAsync(message);
    }
}
