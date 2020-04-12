using System.Collections.Generic;
using async_shell.dependencies.networking.network_resource;
using async_shell.dependencies.serializer;
using async_shell.dependencies.data_holders;


namespace async_shell.dependencies.networking.network_manager
{
    public class SingeResourceNetworkingManager<TData> : INetworkingManager<TData>
    {
        private ITaskScheduler _task_scheduler;
        private IResource _communcation_resource;
        private ISerializer<TData> _serializer;

        private List<int> _tasks_paused_by_user;
        private List<int> _awaiting_to_run_tasks;

        private int _current_running_task = -1;

        public SingeResourceNetworkingManager(IResource communication_resource, ISerializer<TData> serializer)
        {
            this._task_scheduler = new SessionBasedTaskScheduler(communication_resource);
            this._communcation_resource = communication_resource;
            this._serializer = serializer;
            this._tasks_paused_by_user = new List<int>();
        }

        private void nothing()
        {
            System.Console.WriteLine("on re think invoked!");
        }

        private void OnReThink()
        {
            int most_important_task = this._task_scheduler.GetMostImportantTaskID(0);
            if (this._current_running_task != most_important_task && !this._tasks_paused_by_user.Contains(this._current_running_task))
            {
                this._task_scheduler.PauseTaskByID(this._current_running_task); // TODO - need to check what happens when no tasks have to be started.

                if (most_important_task is in this._awaiting_to_run_tasks)
                {
                    this._task_scheduler.StartTaskByID(most_important_task);
                }

                else
                {
                    this._task_scheduler.ResumeTaskByID(most_important_task);
                }
            }
            
            if (this.current_running_task != most_important_task && 
                !this._tasks_paused_by_user.Contains(most_important_task) && 
                this._tasks_allowed_to_run_by_user.Contains(most_important_task))
            {
                this._StopByTaskID(this.current_running_task);
                this.current_running_task = most_important_task;
                this._StartByTaskID(this.current_running_task);
                break;
            }
        }
        
        public int AddDataToSendPool(TData data, int priority)
        {
            /*
            Add data to the resource pool.
            returns the id of the task created.
            */
            byte[] data_buffer = this._serializer.Serialize(data);
            return this._task_scheduler.AddToPool(data_buffer, priority);
        }

        public bool StartByTaskID(int task_id)
        {
            this._awaiting_to_run_tasks.Add(task_id);
            this._task_scheduler.StartTaskByID(task_id);
            return true;
        }

        public bool _ResumeByTaskID(int task_id)
        {
            this._task_scheduler.ResumeTaskByID(task_id);
            return true;
        }

        public bool ResumeByTaskID(int task_id)
        {
            return this._ResumeByTaskID(task_id);
        }

        private bool _PauseByTaskID(int task_id)
        {
            this._task_scheduler.PauseTaskByID(task_id);
            return true;
        }

        public bool PauseByTaskID(int task_id)
        {
            this._tasks_paused_by_user.Add(task_id);
            this._PauseByTaskID(task_id);
            return true;
        }

        public bool CancelByTaskID(int task_id)
        {
            this._task_scheduler.RemoveFromPool(task_id);
            return true;
        }
    }
}