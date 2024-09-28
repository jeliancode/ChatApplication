using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LookMeChatApp
{
    public class MessagesManager
    {
        private readonly Action<string> _messageReceivedCallback;

        public MessagesManager(Action<string> messageReceivedCallback)
        {
            _messageReceivedCallback = messageReceivedCallback;
        }

        public void ReceiveMessages(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[1024];


                while (true)
                {
                    int byteCounter = stream.Read(buffer, 0, buffer.Length);
                    if (byteCounter == 0)
                    {
                        Console.WriteLine("La conexión se ha cerrado.");
                        break;
                    }

                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, byteCounter);
                    Console.WriteLine("Mensaje recibido: " + receivedMessage);

                    _messageReceivedCallback?.Invoke(receivedMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public void SendMessage(NetworkStream stream, string messageContent)
        {
            try
            {
                if (!string.IsNullOrEmpty(messageContent))
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(messageContent);
                    stream.Write(buffer, 0, buffer.Length); 
                    Console.WriteLine("Mensaje enviado: " + messageContent);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public void StartReceiving(NetworkStream stream)
        {
            Thread receiverThread = new Thread(() => ReceiveMessages(stream));
            receiverThread.Start();
        }

        public void CloseConnection(NetworkStream stream)
        {
            if (stream != null)
            {
                stream.Close();
                Console.WriteLine("Conexión cerrada.");
            }
        }
    }
}
