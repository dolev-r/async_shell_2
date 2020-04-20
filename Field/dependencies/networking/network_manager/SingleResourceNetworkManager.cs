using System;
using System.Threading;
using System.Threading.Tasks;
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

        private Semaphore _semaphore = new Semaphore(1,1);

        public SingeResourceNetworkingManager(IResource communication_resource, ISerializer<TData> serializer)
        {
            this._task_scheduler = new SessionBasedTaskScheduler(communication_resource);
            this._communcation_resource = communication_resource;
            this._serializer = serializer;
            this._tasks_paused_by_user = new List<int>();
            this._awaiting_to_run_tasks = new List<int>();
        }

        private int TaskIDToRunNext()
        {
            for (int i = 0; i < this._task_scheduler.AmountOfTasks(); i++)
            {
                int most_important_task = this._task_scheduler.GetMostImportantTaskID(i);
                if (this._current_running_task != most_important_task && !this._tasks_paused_by_user.Contains(this._current_running_task))
                {
                    return most_important_task;
                }
            }
            return -1;
        }

        private void InvokeReThink()
        {
            this._semaphore.Release();
        }

        public void OnReThink()
        {
            while (true)
            {
                this._semaphore.WaitOne();
                int task_id_to_run = this.TaskIDToRunNext();

                if (this._current_running_task != -1) // AKA - no task is running, TODO - look what happens when no task is running 
                {
                    this._task_scheduler.PauseTaskByID(this._current_running_task);
                }

                if (this._awaiting_to_run_tasks.Contains(task_id_to_run))
                {
                    Action<int> a = new Action<int>()
                    var t = Task.Run()
                    this._task_scheduler.StartTaskByID(task_id_to_run);
                    this._current_running_task = task_id_to_run;
                }

                else
                {
                    this._task_scheduler.ResumeTaskByID(task_id_to_run);
                    this._current_running_task = task_id_to_run;
                }
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
            return true;
        }

        public bool ResumeByTaskID(int task_id)
        {
            this._task_scheduler.ResumeTaskByID(task_id);
            return true;
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