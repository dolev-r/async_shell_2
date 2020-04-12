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
        private bool _has_started = false;
        private int _session_id;

        public PausableDataSender(IResource resource, byte[] data_buffer, int session_id)
        {
            this._resource = resource;
            this._data_buffer = data_buffer;
            this._session_id = session_id;
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

        public byte[] Recieve() // TODO - integrate Packet Generator into here
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

        public void Start() 
        {
            this._is_running = true;
            this._has_started = true;
            int real_offset = 0;
            
            PacketGenerator g = new PacketGenerator(this._data_buffer, this._session_id, this._resource.GetDefaultBufferSize());
            foreach (byte[] buffer in g)
            {
                this.send_controll_semaphore.WaitOne();
                
                real_offset += this._resource.Send(buffer);

                byte[] responce = this.Recieve();
                this.send_controll_semaphore.Release();
            }
        
        }
        
        public bool Pause()
        {
            if (this._is_running && this._has_started)
            {
                this.send_controll_semaphore.WaitOne();
                this._is_running = false;
                return true;
            }
            return false;
        }

        public bool Resume()
        {
            if (!this._is_running && this._has_started)
            {
                this.send_controll_semaphore.Release();
                this._is_running = true;
                return true;
            }
            return false;
        }

        public bool HasStarted()
        {
            return this._has_started;
        }
    }
}