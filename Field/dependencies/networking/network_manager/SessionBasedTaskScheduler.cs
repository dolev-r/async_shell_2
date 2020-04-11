using System.Collections.Generic;
using System;
using System.Linq;
using async_shell.dependencies.networking.network_resource;
using async_shell.dependencies.networking.protocol;

namespace async_shell.dependencies.networking.network_manager
{
    public class SessionBasedTaskScheduler : ITaskScheduler
    {   
        private Dictionary<ISingleTask, int> _pool = new Dictionary<ISingleTask, int>();
        private List<ISingleTask> _awaiting_pool = new List<ISingleTask>();
        private ISessionBasedProtocol s;
        private int _task_counter;
        private Action _on_task_finish;

        public SessionBasedTaskScheduler(Action on_task_finish, IResource resource)
        {
            this._task_counter = 0;
            this._on_task_finish = on_task_finish;
            this.s = new SingleResourceSessionBasedProtocol(resource);
        }

        public int AddToPool(byte[] data_buffer, int priority)
        {
            ISingleTask task_to_add = new SingleTask(data_buffer, (task) => this.RemoveFromPool(this._task_counter));
            task_to_add.Priority = priority;
            task_to_add.TaskID = this._task_counter;
            
            int session_id = this.s.AddToPool(task_to_add.GetDataBuffer());
            this._pool.Add(task_to_add, session_id);
            
            this._task_counter += 1;
            return task_to_add.TaskID;
        }

        public int AmountOfTasks()
        {
            return this._pool.Count();
        }

        public int GetMostImportantTaskID(int priority)
        {
            List<ISingleTask> copy = new List<ISingleTask>(this._pool.Keys);
            copy.Sort();
            return copy.ElementAtOrDefault(priority).TaskID;
        }

        private ISingleTask GetTaskFromID(int task_id)
        {
            return this._pool.Keys.Single(task => task.TaskID == task_id);
        }

        public void StartTaskByID(int task_id)
        {
            ISingleTask t = this.GetTaskFromID(task_id);
            int session_id = this._pool[t];
            this.s.Start(session_id);
        }

        public void ResumeTaskByID(int task_id)
        {
            ISingleTask t = this.GetTaskFromID(task_id);
            int session_id = this._pool[t];
            this.s.Resume(session_id);
        }
        
        public void PauseTaskByID(int task_id)
        {
            ISingleTask t = this.GetTaskFromID(task_id);
            int session_id = this._pool[t];
            this.s.Pause(session_id);
        }

        public void RemoveFromPool(int task_id)
        {
            ISingleTask t = this.GetTaskFromID(task_id);
            this._pool.Remove(t);
            this._on_task_finish();
        }

        // public int GetTaskIDViaFunc(Func<DataAttributes, bool> func)
        // {
            
        //     var query = this._pool.Aggregate(task => task.TaskDataAttributes);

        // }

        public bool Any()
        {
            // shoud insert a semaphore.
            if (this._pool.Count > 0)
                return true;
            else
                return false;
        }


    }
}