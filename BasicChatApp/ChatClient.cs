using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BasicChatApp
{
    public class ChatClient
    {
        public static void Main(string[] args)
        {
            MessagesManager messagesManager = new MessagesManager();
            string serverIpAddress = "192.168.1.204";
            int serverPort = 6000;
            try
            {
                TcpClient client = new TcpClient(serverIpAddress, serverPort);
                NetworkStream stream = client.GetStream();
                Thread receiverThread = new Thread(() => messagesManager.ReceiveMessages(stream));
                receiverThread.Start();

                while (true)
                {
                    string message = Console.ReadLine();
                    if (!string.IsNullOrEmpty(message))
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes(message);
                        stream.Write(buffer, 0, buffer.Length);
                        Console.WriteLine("Mensaje enviado: " + message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
