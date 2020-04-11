using async_shell.dependencies.serializer;

namespace async_shell.dependencies.networking.network_from_client.client_protocol.parser_helper
{
    public interface IParserHelper
    {
        ICommand GetCommand(byte[] data, string field_name);
    }
}