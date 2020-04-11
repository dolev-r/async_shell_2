namespace async_shell.dependencies.networking.protocol
{
    public interface ISessionBasedProtocol
    {   
        // returns the session id created.
        int AddToPool(byte[] byte_buffer);

        void Start(int session_id);
        
        void Pause(int session_id);

        void Resume(int session_id);

        byte[] Receive(int session_id);
    }
}