using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.ApplicationLayer.Use_Cases;
public class SubscribToTopicUseCase<A> : IMqttUseCase
{
    private readonly IConnectionHandler<A> _connectionHandler;

    public SubscribToTopicUseCase(IConnectionHandler<A> connectionHandler)
    {
        _connectionHandler = connectionHandler;
    }
    public async Task ExecuteAsync()
    {
        await _connectionHandler.SubscribeToTopicAsync();
    }
}
