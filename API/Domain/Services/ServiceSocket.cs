using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Domain.Services
{
    class SocketService
    {
        public Socket socket = null;

        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder strBuilder = new StringBuilder();

        public ManualResetEvent eventDone = new ManualResetEvent(false);
        public String retorno = "";

        public SocketService()
        {
            socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Connect(String ip, Int32 port)
        {
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ip);
            System.Net.IPEndPoint remoteApp = new IPEndPoint(ipAdd, port);
            socket.Connect(remoteApp);

            socket.BeginReceive(buffer, 0, BufferSize, 0,
                new AsyncCallback(this.ReadCallback), this.socket);

        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            Socket handler = (Socket)ar.AsyncState;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                strBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                content = strBuilder.ToString();

                this.retorno = content;
                eventDone.Set();
            }
        }


        public void Send(String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
            }
            catch (Exception e)
            {
            }
        }

        public void CloseServer()
        {
            socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
        }
    }
}
