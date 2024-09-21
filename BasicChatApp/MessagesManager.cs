using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BasicChatApp
{
    public class MessagesManager
    {
        public void ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) 
                    { 
                        break; 
                    }

                    string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Console.WriteLine("Mensaje recibido: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
