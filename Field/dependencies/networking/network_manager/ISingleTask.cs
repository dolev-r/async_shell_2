using System;
using System.Threading.Tasks;
using async_shell.dependencies.data_holders;

namespace async_shell.dependencies.networking.network_manager
{
    public interface ISingleTask : IComparable
    {
        int Priority { get; set; }
        int TaskID { get; set; }

        byte[] GetDataBuffer();
    }
}