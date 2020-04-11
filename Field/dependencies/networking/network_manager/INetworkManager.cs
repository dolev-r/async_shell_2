using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Generic;
using async_shell.dependencies.data_holders;

namespace async_shell.dependencies.networking.network_manager
{
    public interface INetworkingManager<TData>
    {
        int AddDataToSendPool(TData data, int priority);

        bool StartByTaskID(int task_id);

        bool ResumeByTaskID(int task_id);

        bool PauseByTaskID(int task_id);
        
        bool CancelByTaskID(int task_id);
    }
}