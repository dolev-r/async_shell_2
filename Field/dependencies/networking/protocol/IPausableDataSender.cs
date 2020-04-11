namespace async_shell.dependencies.networking.protocol
{
    public interface IPausableDataSender
    {
        void Start();
        void Pause();
        void Resume();
        byte[] Recieve();
    }
}