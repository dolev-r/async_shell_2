namespace async_shell.dependencies.networking.protocol
{
    public interface IPausableDataSender
    {
        void Start();
        bool Pause();
        bool Resume();
        byte[] Recieve();
    }
}