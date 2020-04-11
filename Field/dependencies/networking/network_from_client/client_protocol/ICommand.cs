using async_shell.dependencies.data_holders;

namespace async_shell.dependencies.networking.network_from_client.client_protocol
{
    public interface ICommand
    {
        string CommandType();

        // preforms the action, such as download file or pause task, returns the data that needs to eb sent as a responce.
        DataWithAttributes PreformAction();
    }
}