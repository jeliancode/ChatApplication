using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Model;
using MQTTnet.Client;

namespace LookMeChatApp.Domain.Interface
{
    public interface IMessageHandler
    {
        Task ConnectToMqttBrokerAsync();
        Task SendMessageAsync(ChatMessage message);
        Task ReceiveMessageAsync(MqttApplicationMessageReceivedEventArgs e);
    }
}
