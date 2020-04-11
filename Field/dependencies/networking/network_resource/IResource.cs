namespace async_shell.dependencies.networking.network_resource
{
    public interface IResource
    {   
        int GetDefaultBufferSize();

        int Send(byte[] byte_buffer);
        
        int Receive(byte[] storage_buffer);
        
        int Receive (byte[] buffer, int offset, int size);
    }
}