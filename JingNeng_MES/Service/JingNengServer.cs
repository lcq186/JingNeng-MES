
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using static JingNeng_MES.Model.CommunicationModel;

namespace JingNeng_MES.Service
{
    internal class JingNengServer
    {
        private int _bufferIndex;
        private JingNengServer _jingNengServer = null;
        private byte[] _receiveBuffer = null;
        private Socket _socket = null;
        public event EventHandler<MesEventArgs> OnRequestHandle;
        public Dictionary<IntPtr, Socket> _DictionarycSockets;
        public JingNengServer()
        {
            _jingNengServer = this;
            _receiveBuffer = new byte[1024];
            _bufferIndex = 1024;
            _DictionarycSockets = new Dictionary<IntPtr, Socket>();
        }

        ///// <summary>
        ///// 剩余的 buffer 数量
        ///// </summary>
        //private int RemainSize => (1024 - _bufferIndex);

        /// <summary>
        /// 绑定ip 和端口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public JingNengServer Bind(string ip, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress iPAddress = IPAddress.Parse(ip);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);

            LoggerHelper._.Debug(iPEndPoint.ToString());
            _socket.Bind(iPEndPoint);

            return _jingNengServer;

        }

        public JingNengServer Listen(int queueCount)
        {
            //0：不处理队列，>0：最多处理的队列长度。
            _socket.Listen(queueCount);

            return _jingNengServer;
        }
        public void Send(string str)
        {
            LoggerHelper._.Debug(str);
            byte[] buffBytes = Encoding.UTF8.GetBytes(str);

            Send(buffBytes);

        }

        public void Send(byte[] buffer)
        {
            var _dir = _DictionarycSockets.LastOrDefault();
            Send(_dir.Key, buffer);

        }
        public void Send(IntPtr handle, byte[] buffer)
        {
            try
            {


                if (_DictionarycSockets.TryGetValue(handle, out var _client))
                {

                    _client.Send(buffer);
                }
                else
                {
                    LoggerHelper._.Warn(handle + "丢失");

                }
            }
            catch (Exception z)
            {
                _DictionarycSockets.Remove((handle));
                LoggerHelper._.Error(handle + "丢失", z);

            }

        }

        public JingNengServer StartAsync()
        {
            _socket.BeginAccept(AsyncAcceptCallback, _socket);

            return _jingNengServer;
        }

        private void AsyncAcceptCallback(IAsyncResult ar)
        {
            //获取自定义的对象
            Socket socket = ar.AsyncState as Socket;
            //异步接受传入的连接尝试，并创建一个新 Socket 来处理远程主机通信。
            Socket clientSocket = socket.EndAccept(ar);



            string helloStr = $"{clientSocket.LocalEndPoint}";
            byte[] helloByte = Encoding.UTF8.GetBytes(helloStr);

            LoggerHelper._.Info($"{helloStr}:{clientSocket.Handle}");
            //  clientSocket.Send(helloByte);
            _DictionarycSockets.Remove(clientSocket.Handle);
            _DictionarycSockets.Add(clientSocket.Handle, clientSocket);

            clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, AsyncReceiveCallback, clientSocket);

            // .BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), ts);


            socket.BeginAccept(AsyncAcceptCallback, socket);
        }


        private void AsyncReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    //接收不到数据，说明客户端已经关闭，那在服务端这里要关闭 Socket
                    // clientSocket.Close();

                    LoggerHelper._.Warn("客户端已经关闭");
                    //return;
                }
                else
                {
                    //_bufferIndex = count;
                    ReadReceiveData(clientSocket.Handle);
                    _receiveBuffer = new byte[_bufferIndex];
                    clientSocket.BeginReceive(_receiveBuffer,
                        0,
                        _bufferIndex,
                        SocketFlags.None,
                        AsyncReceiveCallback,
                        clientSocket);
                }
            }
            catch (Exception e)
            {

                if (clientSocket != null)
                {
                    clientSocket.Close();
                    Console.WriteLine("客户端已经关闭");
                }
                LoggerHelper._.Error(null, e);
            }
            finally
            {
                //有没有异常最后都要做的：
            }
        }

        public void CloseServer()
        {
            _socket.Close();
        }
        private void ReadReceiveData(IntPtr handle)
        {

            int length = _bufferIndex;
            if (length <= 0)
            {
                return;
            }
            string receiveStr = Encoding.UTF8.GetString(_receiveBuffer, 0, length);
            //Array.Copy(_receiveBuffer, 4 + count, _receiveBuffer, 0, _bufferIndex);
            //Console.WriteLine("解析接受到的数据：" + receiveStr);
            OnRequestHandle?.Invoke(this, new MesEventArgs(handle, receiveStr));
        }

    }
}
