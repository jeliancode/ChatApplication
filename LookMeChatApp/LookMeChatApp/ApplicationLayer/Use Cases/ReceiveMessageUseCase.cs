using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.ApplicationLayer.Use_Cases;

public class ReceiveMessageUseCase
{
    private readonly IMessageHandler _messageHandler;

    public ReceiveMessageUseCase(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task ExecuteAsync()
    {
        await _messageHandler.ConnectToMqttBrokerAsync();
    }
}
