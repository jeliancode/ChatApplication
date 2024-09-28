using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LookMeChatApp
{
    public class MessagesManager
    {

        private MessagesManager messagesManager;


        public void ReceiveMessage(NetworkStream stream, string message)
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int byteCounter = stream.Read(buffer, 0, buffer.Length);
                    if (byteCounter == 0)
                    {
                        break;
                    }
                    message = Encoding.ASCII.GetString(buffer, 0, byteCounter);
                }
            }
            catch(Exception e) 
            {
                Console.WriteLine("Exception" + e.Message);
            }
        }

        public void SendMessage(NetworkStream stream, Message message)
        {
            try
            {
                Thread receiverThread = new Thread(() => ReceiveMessage(stream, message.MessageContent));
                receiverThread.Start();

                while (true)
                {
                    
                    if (!string.IsNullOrEmpty(message.MessageContent))
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes(message.MessageContent);
                        stream.Write(buffer, 0, buffer.Length);
                        Console.WriteLine("Mensaje enviado: " + message.MessageContent);
                        
                    }
                    message = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

    }
}
