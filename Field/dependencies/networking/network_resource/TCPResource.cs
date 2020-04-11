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
        
        public int Send(byte[] byte_buffer, int offset, int buffer_size)
        {
            if (buffer_size > this._default_buffer_size)
            {
                throw new System.Exception("buffer size is bigger then allowed");
            }
            if (byte_buffer.Length < buffer_size)
            {
                return this._socket.Send(byte_buffer, offset, byte_buffer.Length, SocketFlags.None);
            }

            return this._socket.Send(byte_buffer, offset, buffer_size, SocketFlags.None);
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

