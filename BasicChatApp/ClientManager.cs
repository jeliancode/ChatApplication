using System;
using System.Net.Sockets;
using System.Text;

namespace BasicChatApp
{
    public class ClientManager
    {
        private NetworkStream senderStream;
        private NetworkStream receiberStream;
        private byte[] buffer;
        private TcpClient _sender;
        private TcpClient _receiber;

        public void HandleClient(TcpClient sender, TcpClient receiber)
        {
            _sender = sender;
            _receiber = receiber;
            senderStream = _sender.GetStream();
            receiberStream = _receiber.GetStream();
            buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int byteCount = senderStream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0)
                    {
                        break;
                    }
                    string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Console.WriteLine("Mensaje recibido: " + message);
                    receiberStream.Write(buffer, 0, byteCount);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        private void CloseConnection()
        {
            senderStream.Close();
            receiberStream.Close();
            _sender.Close();
            _receiber.Close();
        }
    }
}
