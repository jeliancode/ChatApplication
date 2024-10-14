using System;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using LookMeChatApp.Infraestructure.Services;

namespace LookMeChatApp.ApplicationLayer.Use_Cases;

public class SendMessageUseCase<A>
{
    private readonly IConnectionHandler<A> _messageHandler;


    public SendMessageUseCase(IConnectionHandler<A> messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task ExecuteAsync(A message, string currentUserPath)
    {

        await _messageHandler.SendMessageAsync(message, currentUserPath);
    }
}
