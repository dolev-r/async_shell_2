using System;
using System.Linq;
using System.Threading;
using async_shell.dependencies.networking.network_resource;

namespace async_shell.dependencies.networking.protocol
{
    public class PausableDataSender : IPausableDataSender
    {
        private IResource _resource;
        private byte[] _data_buffer;
        private Semaphore send_controll_semaphore = new Semaphore(1, 1);
        private bool _is_running = false;

        public PausableDataSender(IResource resource, byte[] data_buffer, int session_id)
        {
            this._resource = resource;
            this._data_buffer = data_buffer;
        }

        // in case of only listening.
        public PausableDataSender(IResource resource)
        {
            this._resource = resource;
        }
        
        private int CurrentSendSize(int offset, int total_size, int default_buffer_size)
        {
            if (total_size - offset <= default_buffer_size)
            {
                return total_size - offset; 
            }

            return default_buffer_size;
        }

        private int BufferSizeToReceive()
        {
            byte[] buffer = new byte[4];
            this._resource.Receive(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        public byte[] Recieve()
        {
            // Getting the size of the buffer to recieve.
            int data_length = this.BufferSizeToReceive();
            
            byte[] buffer = new byte[data_length];
            
            if (data_length <= this._resource.GetDefaultBufferSize())
            {
                this._resource.Receive(buffer);
            }

            else
            {
                int offset = 0;
                int current_recv_size;
                int default_buffer_size = this._resource.GetDefaultBufferSize();
                int amount_of_bytes_recived;
                while (offset < buffer.Length)
                {
                    current_recv_size = this.CurrentSendSize(offset, buffer.Length, default_buffer_size);
                    amount_of_bytes_recived = this._resource.Receive(buffer, offset, current_recv_size);
                    offset += amount_of_bytes_recived;
                }

            }
            
            return buffer;
        }

        private byte[] Header()
        {
            return BitConverter.GetBytes(this._data_buffer.Length);
        }

        private byte[] CurrentSend(int offset, int send_buffer_size) // TODO - should put this in a class.
        {

        }

        public void Start() // TODO - add enumerable
        {
            this._is_running = true;

            // Sending the buffer size before actually sending the buffer.
            byte[] header = this.Header();
            this._data_buffer = header.Concat(this._data_buffer).ToArray();

            int offset = 0;
            int current_send_size;
            int default_buffer_size = this._resource.GetDefaultBufferSize();
            int amount_of_bytes_sent;
            byte[] actually_send;
            while (offset < this._data_buffer.Length)
            {
                this.send_controll_semaphore.WaitOne();

                current_send_size = this.CurrentSendSize(offset, this._data_buffer.Length, default_buffer_size);

                Index Start = offset;
                Index End = offset + current_send_size;
                actually_send = this._data_buffer[Start..End];

                amount_of_bytes_sent = this._resource.Send(actually_send);
                offset += amount_of_bytes_sent;

                byte[] responce = this.Recieve();
                this.send_controll_semaphore.Release();
            }

        } 
        
        public void Pause()
        {
            if (this._is_running)
            {
                this.send_controll_semaphore.WaitOne();
                this._is_running = false;
            }
        }

        public void Resume()
        {
            if (this._is_running)
            {
                this.send_controll_semaphore.Release();
                this._is_running = true;
            }
        }
    }
}