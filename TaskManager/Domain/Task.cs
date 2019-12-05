using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//for this project
using System.Collections;

namespace TaskManager.Domain
{
    public class Task
    {
        //define task properties
        public Int32 TaskID { get; set; }
        public String TaskName { get; set; }
        public String Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public String AssignTo { get; set; }
        public String Status { get; set; }
        public String CreateBy { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public String UpdateBy { get; set; }
        public DateTime? UpdateDatetime { get; set; }

        //constructor
        public Task()
        { }

        public Task(Int32 taskID,String taskName,String description,DateTime? startDate,DateTime? dueDate,DateTime? completeDate,
                    String assignTo,String status,String createBy,DateTime createDatetime,String updateBy,DateTime updateDatetime)
        {
            this.TaskID = taskID;
            this.TaskName = taskName;
            this.Description = description;
            this.StartDate = startDate;
            this.DueDate = dueDate;
            this.CompleteDate = completeDate;
            this.AssignTo = assignTo;
            this.Status = status;
            this.CreateBy = createBy;
            this.CreateDatetime = createDatetime;
            this.UpdateBy = updateBy;
            this.UpdateDatetime = updateDatetime;
        }
    }
}
