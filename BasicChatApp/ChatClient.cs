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
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 6000);
                NetworkStream stream = client.GetStream();
                Thread receiverThread = new Thread(() => ReceiveMesseges(stream));
                receiverThread.Start();

                while (true)
                {
                    string message = Console.ReadLine();
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch(Exception e) 
            {
                Console.WriteLine("Error: " + e.Message);
            }
    }
}
