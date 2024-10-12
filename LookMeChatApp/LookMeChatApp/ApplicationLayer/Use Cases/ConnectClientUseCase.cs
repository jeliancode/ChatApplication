using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.ApplicationLayer.Use_Cases;

public class ConnectClientUseCase : IMqttUseCase
{
    private readonly IConnectionHandler _connectionHandler;

    public ConnectClientUseCase(IConnectionHandler connectionHandler)
    {
        _connectionHandler = connectionHandler;
    }

    public async Task ExecuteAsync()
    {
        await _connectionHandler.ConnectToMqttBrokerAsync();
    }
}
