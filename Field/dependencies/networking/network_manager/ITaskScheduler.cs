using async_shell.dependencies.networking.network_resource;

namespace async_shell.dependencies.networking.network_manager
{
    public interface ITaskScheduler
    {
        int GetMostImportantTaskID(int priority);

        int AmountOfTasks();

        int AddToPool(byte[] data_buffer, int priority);
        
        void StartTaskByID(int task_id);

        bool ResumeTaskByID(int task_id);
        
        bool PauseTaskByID(int task_id);

        void RemoveFromPool(int task_id);

        bool Any();
    }
}