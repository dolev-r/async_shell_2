using System.Net;
using System.Net.Sockets;

namespace async_shell.dependencies.networking.network_resource
{
    public class TcpResource : IResource
    {
        private Socket _socket;
        
        private int _default_buffer_size;
        private IPAddress ip_address;
        private int port;


        public TcpResource(int default_buffer_size, IPAddress ip_address, int port)
        {
            IPEndPoint local_address = new IPEndPoint(ip_address, port);
            this._socket = new Socket(ip_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this._socket.Connect(local_address);

            this._default_buffer_size = default_buffer_size;
        }

        public TcpResource(int default_buffer_size, Socket socket)
        {
            this._socket = socket;
            this._default_buffer_size = default_buffer_size;
        }
        
        public int GetDefaultBufferSize()
        {
            return this._default_buffer_size;
        }
        
        public int Send(byte[] byte_buffer)
        {
            if (byte_buffer.Length > this._default_buffer_size)
            {
                throw new System.Exception("buffer size is bigger then allowed");
            }

            return this._socket.Send(byte_buffer, 0, byte_buffer.Length, SocketFlags.None);
        }

        public int Receive(byte[] buffer)
        {
            return this._socket.Receive(buffer);
        }

        public int Receive (byte[] buffer, int offset, int size)
        {
            return this._socket.Receive(buffer, offset, size, SocketFlags.None);
        }
    }
}

