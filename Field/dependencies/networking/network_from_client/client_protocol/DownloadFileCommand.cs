using System.Text;
using async_shell.dependencies.networking.network_manager;
using async_shell.dependencies.data_holders;

namespace async_shell.dependencies.networking.network_from_client.client_protocol
{
    public class DownloadFileCommand : ICommand
    {
        public string FilePath;
        
        
        private string _command_type = "DownloadFileCommand";

        public string CommandType()
        {
            return this._command_type;
        }

        public DataWithAttributes PreformAction()
        {
            // download the file.
            FileData file_data = new FileData();
            file_data.Name = this.FilePath;
            file_data.Content = Encoding.UTF8.GetBytes("asdasdasdasdasdasdasdasd");
            file_data.Size = 10;
            return file_data;
        }
    }
}