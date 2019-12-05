using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//for this project
using TaskManager.Domain;
using System.Data;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // property
        Boolean isLoading = false;
        List<String> lstStatus = new List<String>();

        // setup default status value in list
        private void SetupDefaultStatus()
        {
            lstStatus.Add("Active");
            lstStatus.Add("Completed");
            lstStatus.Add("Overdue");
            lstStatus.Add("Not Started");
        }

        public MainWindow()
        {
            isLoading = true;
            InitializeComponent();

            this.datagridTask.CanUserAddRows = false;
            SetupDefaultStatus();
            PopulateTasks(false);
            this.taskName.Focus();

            isLoading = false;
        }

        // get all tasks when form loading
        private void PopulateTasks(Boolean onlyCurrentTasks)
        {
            //load all tasks to the datagrid
            TaskManagement taskMgt = new TaskManagement();
            try
            {                
                datagridTask.Columns.Clear();

                
                //get all tasks by current date
                datagridTask.ItemsSource = taskMgt.GetTasks(onlyCurrentTasks).GetAllRecords();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Get all tasks errors occured", ex.Message);
            }
        }

     
        // add a new task
        
        private void newTask_Click(object sender, RoutedEventArgs e)
        {
            //when click add a new task then clear all textboxes
            clearAllTexts();
        }

        //clear all textboxes
        private void clearAllTexts()
        {
            taskID.Text = "";
            taskName.Text = "";
            taskDescription.Text = "";
            taskAssignTo.Text = "";
            status.Text = "";
            startDate.Text = "";
            dueDate.Text = "";
            completeDate.Text = "";
        }

        //check if text fields which should not be null in the task manager database. if null then reminder use to input it
        private Boolean  checkAllTexts()
        {
            Boolean istrue=false;
            if (taskName.Text == "")
            {
                MessageBox.Show("Please input the task name!");
                taskName.Focus();
            }
            else if (startDate.Text == "")
            {
                MessageBox.Show("Please select a start date!");
                startDate.Focus();
            }
            else if (status.Text == "")
            {
                MessageBox.Show("Please select a status!");
                status.Focus();
            }
            else
                istrue= true;
            return istrue;
        }

        //save a task according add a new one or update the existing one. if task name, description, assign to, status, startdate,duedate ,completedate all same then update it else add a new one
        private void saveTask_Click(object sender, RoutedEventArgs e)
        {
            String strReturn="";
            strReturn=checkIfAddOrUpdate();
            if (checkAllTexts())
            {
                TaskManagement taskMgt = new TaskManagement();

                String txtName = taskName.Text.Trim();
                String txtDesc = taskDescription.Text.Trim();
                String txtAssignTo = taskAssignTo.Text.Trim();
                String cmbStatus = status.Text;
                DateTime? dtStartDate = startDate.SelectedDate;
                DateTime? dtDuedate = dueDate.SelectedDate;
                DateTime? dtCompleteDate = completeDate.SelectedDate;


                if (strReturn == "insert")
                {
                    String txtCreateBy = Environment.UserName;
                    DateTime dtCreateDate = DateTime.Now;
                    taskMgt.AddTask(txtName, txtDesc, dtStartDate, dtDuedate, dtCompleteDate, txtAssignTo, cmbStatus, txtCreateBy, dtCreateDate);
                }
                if (strReturn == "update")
                {
                    String txtTaskID = taskID.Text.Trim();
                    String txtUpdateBy = Environment.UserName;
                    DateTime dtUpdateDate = DateTime.Now;
                    Boolean updateDone= taskMgt.UpdateTask(Int32.Parse(txtTaskID), txtName, txtDesc, dtStartDate, dtDuedate, dtCompleteDate, txtAssignTo, cmbStatus, txtUpdateBy, dtUpdateDate);
                    if (updateDone)
                    {
                        MessageBox.Show("Update is done!","Message");
                    }
                }

                isLoading = true;
                PopulateTasks(false);
                isLoading = false;
                datagridTask.Items.Refresh();
            }
           
            
        }

        private String checkIfAddOrUpdate()
        {
            if (taskID.Text.Trim() != "")

            {
                return "update";
            }
            else
                return "insert";
            
            
        }

        //delete a task according task id
        private void deleteTask_Click(object sender, RoutedEventArgs e)
        {
            if( MessageBox.Show("Are you sure you want to delete this recode?","Alert",MessageBoxButton.YesNo)== MessageBoxResult.Yes )
            {
                if (checkTaskID())
                {
                    String txtTaskID = taskID.Text.Trim();
                    TaskManagement taskMgt = new TaskManagement();

                    taskMgt.DeleteTask(Int32.Parse(txtTaskID));
                    isLoading = true;
                    PopulateTasks(false);
                    isLoading = false;
                    datagridTask.Items.Refresh();
                }

            }

        }

        private Boolean checkTaskID()
        {
            Boolean getID = false;

            if (taskID.Text =="")
            {
                MessageBox.Show ("Please choose one task to delete!");
            }
            else
            {
                TaskManagement taskMgt = new TaskManagement();
                Int32 getResult = taskMgt.GetTasksByTaskID(Int32.Parse(taskID.Text.Trim())).Count();
                if (getResult > 0)
                    getID = true;

            }

            return getID;
        }

        private void datagridTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoading)
            {
                Domain.Task currentTask = datagridTask.SelectedItem as Domain.Task;
                taskID.Text = currentTask.TaskID.ToString();
                taskName.Text = currentTask.TaskName.ToString();
                taskDescription.Text = currentTask.Description.ToString();
                startDate.SelectedDate = currentTask.StartDate;
                dueDate.SelectedDate = currentTask.DueDate;
                completeDate.SelectedDate = currentTask.CompleteDate;
                taskAssignTo.Text = currentTask.AssignTo.ToString();
                status.Text = currentTask.Status.ToString();
            }
        }

        

        private void GetTasksStatus()
        {
            TaskManagement taskMgt = new TaskManagement();
            Boolean currentCheck = false;
            if (this.ShowCurrentTasks.IsChecked ==true )
            {
                currentCheck = true;
            }
            datagridTask.ItemsSource=taskMgt.GetTasksByStatus (lstStatus, currentCheck).GetAllRecords ();
            datagridTask.Items.Refresh();
        }
        
        private void datagridTask_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            if (e.Row.Item.GetType().Name == "Task")
            {
                Domain.Task currentTask = (Domain.Task)e.Row.Item;
                if (currentTask.Status == "Completed")
                {
                    e.Row.Background = Brushes.Green;
                }
                else if (currentTask.Status == "Overdue")
                {
                    e.Row.Background = Brushes.Red;

                }
                else
                {
                    e.Row.Background = Brushes.White;
                }

            }
        }

       

        private void statusActive_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoading)
            {
                if (statusActive.IsChecked == true)
                {
                    lstStatus.Add("Active");
                }
                else
                {
                    lstStatus.Remove("Active");
                }
                GetTasksStatus();
            }
        }

        private void statusOverdue_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoading)
            {
                if (statusOverdue.IsChecked == true)
                {
                    lstStatus.Add("Overdue");
                }
                else
                {
                    lstStatus.Remove("Overdue");
                }
                GetTasksStatus();
            }
        }

        private void statusCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoading)
            {
                if (statusCompleted.IsChecked == true)
                {
                    lstStatus.Add("Completed");
                }
                else
                {
                    lstStatus.Remove("Completed");
                }
                GetTasksStatus();
            }
        }

        private void statusNotStarted_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoading)
            {
                if (statusNotStarted.IsChecked == true)
                {
                    lstStatus.Add("Not Started");
                }
                else
                {
                    lstStatus.Remove("Not Started");
                }
                GetTasksStatus();
            }
        }

        private void ShowCurrentTasks_Click(object sender, RoutedEventArgs e)
        {
            if (this.ShowCurrentTasks.IsChecked == true)
            {
                this.PopulateTasks(true);
            }
            else
            {
                this.PopulateTasks(false);
            }
            
        }
    }
}
