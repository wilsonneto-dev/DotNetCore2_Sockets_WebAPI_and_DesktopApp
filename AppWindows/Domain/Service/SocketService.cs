using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppWindows.Domain.Service
{
    class SocketService
    {
        public Socket socket = null;

        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder strBuilder = new StringBuilder();
        public static ManualResetEvent eventDone = new ManualResetEvent(false);

        public String retorno = "";
        public static ManualResetEvent eventRetorno = new ManualResetEvent(false);

        public SocketService()
        {
            socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void RunServer(Int32 port)
        {

            IPAddress hostIP = (Dns.GetHostEntry("127.0.0.1")).AddressList[0];
            IPEndPoint ep = new IPEndPoint(hostIP, port);
            this.socket.Bind(ep);
            this.socket.Listen(10);

            while (true)
            {
                eventDone.Reset();
                socket.BeginAccept(new AsyncCallback(ConnectionInitCallback), socket);
                eventDone.WaitOne();
            }

        }

        public void ConnectionInitCallback(IAsyncResult ar)
        {

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            this.socket= handler;
            handler.BeginReceive( buffer, 0, BufferSize, 0,
                new AsyncCallback(this.ReadCallback), this.socket);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {

                String content = String.Empty;
                Socket handler = (Socket)ar.AsyncState;
                strBuilder.Clear();
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    strBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    content = strBuilder.ToString();

                    Domain.Service.CommandService.Process(content);

                }

                handler.Disconnect(true);
                eventDone.Set();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
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
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket.Dispose();
        }
    }
}
