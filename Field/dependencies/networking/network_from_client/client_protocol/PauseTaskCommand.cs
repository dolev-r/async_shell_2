using async_shell.dependencies.networking.network_manager;
using async_shell.dependencies.data_holders;

namespace async_shell.dependencies.networking.network_from_client.client_protocol
{
    public class PauseTaskCommand : ICommand
    {
        public int TaskID;
        
        private string _command_type = "PauseTaskCommand";

        public string CommandType()
        {
            return this._command_type;
        }

        public DataWithAttributes PreformAction()
        {
            // download the file.
            AckData ack_data = new AckData();
            
            return ack_data;
        }
    }
}