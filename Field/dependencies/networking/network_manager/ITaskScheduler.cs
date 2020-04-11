using async_shell.dependencies.networking.network_resource;

namespace async_shell.dependencies.networking.network_manager
{
    public interface ITaskScheduler
    {
        int GetMostImportantTaskID(int priority);

        int AmountOfTasks();

        int AddToPool(byte[] data_buffer, int priority);
        
        void StartTaskByID(int task_id);

        void ResumeTaskByID(int task_id);
        
        void PauseTaskByID(int task_id);

        void RemoveFromPool(int task_id);

        bool Any();
    }
}