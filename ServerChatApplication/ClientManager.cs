using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    public class ClientManager
    {
        public void HandleClient(TcpClient sender, TcpClient receiver)
        {

            NetworkStream senderStream = sender.GetStream();
            NetworkStream receiberStream = receiver.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int byteCount = senderStream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0)
                    {
                        Console.WriteLine("El cliente ha cerrado la conexión.");
                        break;
                    }

                    string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Console.WriteLine("Mensaje recibido: " + message);

                    receiberStream.Write(buffer, 0, byteCount);
                    Console.WriteLine("Mensaje reenviado al otro cliente.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            finally
            {
                CloseConnection(senderStream, receiberStream);
            }
        }

        private void CloseConnection(NetworkStream senderStream, NetworkStream receiverStream)
        {
            senderStream.Close();
            receiverStream.Close();
        }
    }
}