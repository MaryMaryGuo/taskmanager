using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//for this project
using System.Data;
namespace TaskManager.Domain
{
    public class Tasks
    {
        //properties
        List<Task> _tasks = null;

        //constructor
        public Tasks()
        {
            _tasks = new List<Task>();
        }

        //methods
        //add a new task
        public void Add(Task task)
        {
            this._tasks.Add(task);
        }
        
        //delete a task
        public void Delete(Task task)
        {
            this._tasks.Remove(task);
        }
        
        //count all tasks
        public Int32 Count()
        {
            return this._tasks.Count;
        }
        public List<Task> GetAllRecords()
        {
            return this._tasks;
        }
    }
}
