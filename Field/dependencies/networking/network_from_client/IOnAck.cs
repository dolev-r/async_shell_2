namespace async_shell.dependencies.networking.network_from_client
{
    public interface IOnAck
    {
        void OnAckFromClient(byte[] data_from_client);
    }
}