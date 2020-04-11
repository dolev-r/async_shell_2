using async_shell.dependencies.data_holders;
using async_shell.dependencies.networking.network_manager;
using async_shell.dependencies.serializer;
using async_shell.dependencies.networking.network_from_client.client_protocol.parser_helper;
using async_shell.dependencies.networking.network_from_client.client_protocol;


namespace async_shell.dependencies.networking.network_from_client
{
    public class OnAck : IOnAck
    {
        private INetworkingManager<DataWithAttributes> _network_manager;
        private IParserHelper _parser_helper;
        private string _command_type_identifyer = "CommandType";

        public OnAck(INetworkingManager<DataWithAttributes> network_manager)
        {
            this._network_manager = network_manager;
            _parser_helper = new JsonParserHelper();
        }
        
        public void OnAckFromClient(byte[] data_from_client)
        {
            ICommand command = this._parser_helper.GetCommand(data_from_client, this._command_type_identifyer);
            DataWithAttributes action_result = command.PreformAction();
        }
    }
}