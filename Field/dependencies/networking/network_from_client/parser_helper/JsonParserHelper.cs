using async_shell.dependencies.serializer;

namespace async_shell.dependencies.networking.network_from_client.client_protocol.parser_helper
{
    public class JsonParserHelper : IParserHelper
    {
        private IExtractor _extractor = new JsonExtractor();

        public ICommand GetCommand(byte[] data, string field_name)
        {
            ICommand command = null;
            string command_type = this._extractor.Extract(data, field_name);
            switch (command_type)
            {
                case "DownloadFileCommand":
                    command = (new JsonSerializer<DownloadFileCommand>()).Deserialize(data);
                    break;

                case "PauseTaskCommand":
                    command = (new JsonSerializer<PauseTaskCommand>()).Deserialize(data);
                    break;
            }

            return command;
        }
    }
}