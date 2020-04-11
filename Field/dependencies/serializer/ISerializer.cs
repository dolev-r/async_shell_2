namespace async_shell.dependencies.serializer
{
    public interface ISerializer<TData>
    {
        byte[] Serialize(TData data);

        TData Deserialize(byte[] bytes);
    }
}
