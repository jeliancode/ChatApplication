using System;
using System.Net;
using System.Net.Sockets;
namespace BasicChatApp
{
    public class Server
    {
        private static TcpListener _listener;
        private static TcpClient _client1;
        private static TcpClient _client2;
        private static ClientManager _clientManager;

        public static void Main(string[] args)
        {
            _clientManager = new ClientManager();
            _listener = new TcpListener(IPAddress.Any, 6000);
            _listener.Start();
            Console.WriteLine("Esperando conexiones de clientes");

            _client1 = _listener.AcceptTcpClient();
            Console.WriteLine("1 cliente conectado");

            _client2 = _listener.AcceptTcpClient();
            Console.WriteLine("w clientes conectados");

            Thread client1Thread = new Thread(() => _clientManager.HandleClient(_client1, _client2));
            client1Thread.Start();

            Thread client2Thread = new Thread(() => _clientManager.HandleClient(_client2, _client1));
            client2Thread.Start();
        }

       
    }
}
