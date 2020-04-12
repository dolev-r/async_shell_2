using System.Collections.Generic;
using async_shell.dependencies.networking.network_resource;

namespace async_shell.dependencies.networking.protocol
{
    public class SingleResourceSessionBasedProtocol : ISessionBasedProtocol
    {   
        private Dictionary<int, IPausableDataSender> _pool = new Dictionary<int, IPausableDataSender>();
        private IResource _resource;
        private int _session_id_counter;
        
        public SingleResourceSessionBasedProtocol(IResource resource)
        {
            this._resource = resource;
            this._session_id_counter = 0;
        }

        public int AddToPool(byte[] byte_buffer)
        {
            IPausableDataSender r = new PausableDataSender(this._resource, byte_buffer, this._session_id_counter);
            this._pool.Add(this._session_id_counter, r);
            this._session_id_counter += 1;
            return this._session_id_counter - 1;
        }

        public void Start(int session_id)
        {
            this._pool[session_id].Start();
        }
        
        public bool Pause(int session_id)
        {
            return this._pool[session_id].Pause();
        }

        public bool Resume(int session_id)
        {
            return this._pool[session_id].Resume();
        }

        public byte[] Receive(int session_id)
        {
            return this._pool[session_id].Recieve();
        }
    }
}