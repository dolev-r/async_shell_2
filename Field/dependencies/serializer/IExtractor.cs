namespace async_shell.dependencies.serializer
{
    public interface IExtractor
    {
        string Extract(byte[] data, string fieldName);
    }
}