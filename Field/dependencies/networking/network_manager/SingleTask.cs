using System;
using System.Threading.Tasks;
using async_shell.dependencies.networking.network_resource;

namespace async_shell.dependencies.networking.network_manager
{
    public class SingleTask : ISingleTask
    {        
        private byte[] _data_buffer;
        private Action<Task> _on_finish;
        
        public SingleTask(byte[] data_buffer, Action<Task> on_finish)
        {
            this._data_buffer = data_buffer;
            this._on_finish = on_finish;
        }

        public int TaskID { get; set; }
        public int Priority { get; set; }

        public byte[] GetDataBuffer()
        {
            return this._data_buffer;
        }

        int IComparable.CompareTo(object other)
        {
            SingleTask otherSingleTask = other as SingleTask;
            if (otherSingleTask != null) 
            {
                if (otherSingleTask.Priority > this.Priority)
                    return -1;
                else if (otherSingleTask.Priority == this.Priority)
                    return 0;
                else
                    return 1;
            }
            else
                throw new ArgumentException("Object is not a SingleTask");
            
        }
    }
}