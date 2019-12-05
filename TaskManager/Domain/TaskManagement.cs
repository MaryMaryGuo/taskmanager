using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//for this project
using System.Data;
using TaskManager.DAL;
using System.Data.SqlClient;

namespace TaskManager.Domain
{
    public class TaskManagement
    {
        //constructor

        public TaskManagement()
        { }

        //add a new task
        public Int32 AddTask(String taskName, String Description, DateTime? startDate, DateTime? dueDate, DateTime? completeDate,
                            String assignTo, String Status, String createBy, DateTime? createDatetime)
        {
            Int32 taskID = 0;
            try
            {
                DBObject taskDB = new DBObject();
                List<IDataParameter> pars = new List<IDataParameter>();
                IDataParameter par;


                //assign all fields in the task table
                par = taskDB.CreateParameter;
                par.ParameterName = "@partaskName";
                par.DbType = DbType.String;
                par.Value = taskName;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@pardescription";
                par.Value = Description;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@parstartDate";
                par.Value = startDate;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime ;
                par.ParameterName = "@pardueDate";
                if (dueDate.HasValue)
                { par.Value = dueDate; }
                else
                { par.Value = DBNull.Value; }
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@parcompleteDate";
                if (completeDate.HasValue)
                { par.Value = completeDate; }
                else
                { par.Value = DBNull.Value; }
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parassignTo";
                par.Value = assignTo;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parstatus";
                par.Value = Status;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parcreateBy";
                par.Value = createBy;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@parcreateDatetime";                
                par.Value = createDatetime;
                pars.Add(par);

                
                //insert a new task query
                string sql = @"insert into task(taskname, description, startdate, duedate, completedate," +
                               "assignto, status, createby, createdatetime) " +
                                " output inserted.taskid" +
                               " values(@partaskName, @pardescription, @parstartDate, @pardueDate, @parcompleteDate, " +
                               "@parassignTo, @parstatus, @parcreateBy, @parcreateDatetime)";
                taskID = (Int32) taskDB.ExecuteScalar(sql, pars, CommandType.Text, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return taskID;
        }
        //update an existing task
        public bool UpdateTask(Int32 taskID,String taskName, String Description, DateTime? startDate, DateTime? dueDate, DateTime? completeDate,
                                String assignTo, String Status, String updateBy, DateTime? updateDatetime)
        {
            bool isSuccessful = true;
            try
            {

                DBObject taskDB = new DBObject();
                List<IDataParameter> pars = new List<IDataParameter>();
                IDataParameter par;

                //assign all value to the parameter
                par = taskDB.CreateParameter;
                par.DbType = DbType.Int32;
                par.ParameterName = "@partaskID";
                par.Value = taskID;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String ;
                par.ParameterName = "@partaskName";
                par.Value = taskName;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parDescription";
                par.Value = Description;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime ;
                par.ParameterName = "@parstartDate";
                par.Value = startDate;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@parduedate";
                par.Value = dueDate;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@parcompleteDate";
                if (completeDate.HasValue)
                { par.Value = completeDate; }
                else
                { par.Value = DBNull.Value; }
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parassignTo";
                par.Value = assignTo;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parstatus";
                par.Value = Status;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.String;
                par.ParameterName = "@parupdateBy";
                par.Value = updateBy;
                pars.Add(par);

                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@parupdateDatetime";
                par.Value = updateDatetime;
                pars.Add(par);

                //update an existing task according task id               

                string sql = "update task set taskname=@partaskName, description=@parDescription, startdate=@parstartDate," +
                              "duedate=@parduedate, completedate=@parcompleteDate, assignto=@parassignTo, status=@parstatus, " +
                              "updateby=@parupdateBy, updatedatetime=@parupdateDatetime where taskid=@partaskID";

                taskDB.ExecuteTransaction (sql, pars, CommandType.Text, true);
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                throw new Exception(ex.Message);
            }

            return isSuccessful;
        }
        //delete an existing task
        public bool DeleteTask(Int32 taskID)
        {
            bool isSuccessful = true;
            try
            {
                DBObject taskDB = new DBObject();
                List<IDataParameter> pars = new List<IDataParameter>();
                IDataParameter par = taskDB.CreateParameter;

                par.ParameterName = "@taskid";
                par.Value = taskID;
                pars.Add(par);

                string sql = @"delete from task where taskid=@taskID";
                taskDB.ExecuteTransaction(sql, pars, CommandType.Text, true);
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                throw new Exception(ex.Message);
            }

            return isSuccessful;
        }
        
        // get nullable date time
        private DateTime? GetNullableDatetime(IDataReader dr, String fieldName)
        {         
            try
            {
                return dr.GetDateTime(dr.GetOrdinal(fieldName));
            }
            catch
            {
                return null;
            }           
        }
        // get nullable string
        private String GetNullableString(IDataReader dr, String fieldName)
        {
            try
            {
                return dr.GetString(dr.GetOrdinal(fieldName));
            }
            catch
            {
                return "";
            }
        }

        //get all tasks data list
        public Tasks GetTasks(Boolean onlyCurrentTasks)
        {
            Tasks alltasks = new Tasks();
            String sqlcurrent ;
            List<IDataParameter> pars = new List<IDataParameter>();
            IDataParameter par;
            TaskManager.DAL.DBObject taskDB = new TaskManager.DAL.DBObject();


            if (onlyCurrentTasks)
            {
                sqlcurrent = "select * from task where startdate<@dtCurrent and duedate>@dtCurrent order by taskid asc";
                
                par = taskDB.CreateParameter;
                par.DbType = DbType.DateTime;
                par.ParameterName = "@dtCurrent";
                par.Value = DateTime.Now;
                pars.Add(par);
            }
            else
            {
                sqlcurrent = "select * from task order by taskid asc";
            }

            

            Task task = null;
            using (IDataReader dr = taskDB.ExecuteQuery(sqlcurrent, pars, CommandType.Text, true))
            {
                while (dr.Read())
                {

                    task = new Task();
                    task.TaskID = dr.GetInt32(dr.GetOrdinal("taskid"));
                    task.TaskName = dr.GetString(dr.GetOrdinal("taskname"));
                    task.Description = dr.GetString(dr.GetOrdinal("description"));
                    task.StartDate = this.GetNullableDatetime(dr,"startdate");
                    task.DueDate = this.GetNullableDatetime(dr, "duedate");
                    task.CompleteDate = this.GetNullableDatetime(dr, "completedate");

                    task.AssignTo = dr.GetString(dr.GetOrdinal("assignto"));
                    task.Status = dr.GetString(dr.GetOrdinal("status"));
                    task.CreateBy = dr.GetString(dr.GetOrdinal("createby"));                    
                    task.CreateDatetime = dr.GetDateTime(dr.GetOrdinal("createdatetime"));     
                      
                    task.UpdateBy = this.GetNullableString(dr,"updateby");
                    task.UpdateDatetime = this.GetNullableDatetime(dr, "updatedatetime");
                    
                    alltasks.Add(task);
                }
            }

            return alltasks;

        }

        //get all tasks according task id
        public Tasks GetTasksByTaskID(Int32? taskID)
        {
            Tasks alltasks = new Tasks();
            DBObject taskDB = new DBObject();
            List<IDataParameter> pars = new List<IDataParameter>();
            IDataParameter par = taskDB.CreateParameter;

            Task task = null;

            par.ParameterName = "@taskid";
            par.Value = taskID;
            pars.Add(par);

            using (IDataReader dr = taskDB.ExecuteQuery("select * from task where taskid=@taskID order by taskid desc", pars, CommandType.Text, true))
            {
                
                while (dr.Read())
                {
                    task = new Task();
                    task.TaskID = dr.GetInt32(dr.GetOrdinal("taskid"));
                    task.TaskName = dr.GetString(dr.GetOrdinal("taskname"));
                    task.Description = dr.GetString(dr.GetOrdinal("description"));
                    task.StartDate = dr.GetDateTime(dr.GetOrdinal("startdate"));
                    task.DueDate = this.GetNullableDatetime(dr, "duedate");
                    task.CompleteDate = this.GetNullableDatetime(dr, "completedate");
                    task.AssignTo = dr.GetString(dr.GetOrdinal("assignto"));
                    task.Status = dr.GetString(dr.GetOrdinal("status"));
                    task.CreateBy = dr.GetString(dr.GetOrdinal("createby"));
                    task.CreateDatetime = dr.GetDateTime(dr.GetOrdinal("createdatetime"));
                    task.UpdateBy = this.GetNullableString(dr, "updateby");
                    task.UpdateDatetime = this.GetNullableDatetime(dr, "updatedatetime");

                    alltasks.Add(task);
                }
            }

            return alltasks;

        }

        //get all tasks by parameter query
        public Tasks GetTasksByStatus(List<String> lstStatus,Boolean currentOrNot)
        {

            Tasks alltasks = new Tasks();
          
            DBObject taskDB = new DBObject();

            

            String condition = "";
            for (int i=0; i<lstStatus.Count;i++)
            {
                if (i!= lstStatus.Count - 1)
                {
                    condition = condition+ " status='" + lstStatus[i] + "' or ";
                }
                else 
                {
                    condition = condition+" status='" + lstStatus[i]+"'";
                }
                
            }

            if (currentOrNot)
            {
                condition = condition + " and startdate<GETDATE() and duedate>GETDATE()";
            }

            String sqlStaus;

            if (condition=="")
            {
                sqlStaus = "select * from task";
            }
            else 
            {
                sqlStaus = "select * from task where " + condition;
            }
            

            Task task = null;

            using (IDataReader dr = taskDB.ExecuteQuery(sqlStaus ,null, CommandType.Text, true))
            {
               
                while (dr.Read())
                {
                    task = new Task();
                    task.TaskID = dr.GetInt32(dr.GetOrdinal("taskid"));
                    task.TaskName = dr.GetString(dr.GetOrdinal("taskname"));
                    task.Description = dr.GetString(dr.GetOrdinal("description"));
                    task.StartDate = dr.GetDateTime(dr.GetOrdinal("startdate"));
                    task.DueDate = this.GetNullableDatetime(dr, "duedate");
                    task.CompleteDate = this.GetNullableDatetime(dr, "completedate");
                    task.AssignTo = dr.GetString(dr.GetOrdinal("assignto"));
                    task.Status = dr.GetString(dr.GetOrdinal("status"));
                    task.CreateBy = dr.GetString(dr.GetOrdinal("createby"));
                    task.CreateDatetime = dr.GetDateTime(dr.GetOrdinal("createdatetime"));
                    task.UpdateBy = this.GetNullableString(dr, "updateby");
                    task.UpdateDatetime = this.GetNullableDatetime(dr, "updatedatetime");

                    alltasks.Add(task);
                }
            }

            return alltasks;

        }
    }
}
