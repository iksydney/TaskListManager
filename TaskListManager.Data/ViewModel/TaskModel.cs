namespace TaskListManager.Data.ViewModel
{
    public class TaskModel
    {
        public string DateCreated { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string StartDate { get; set; }
        public int AllotedTime { get; set; } // in days
        public int ElapsedTime { get; set; } // in days
        public bool TaskStatus { get; set; } // false = PENDING, true = CLOSED
        public string Id { get; set; }
    }

    public class TaskViewModel
    {
        public string Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDate { get; set; }
        public int AllotedTime { get; set; }
        public int ElapsedTime { get; set; }
        public string Status { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public int DaysLate { get; set; }
    }

    public class CreateTaskDto
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDate { get; set; }
        public int AllotedTime { get; set; }
    }

    public class UpdateTaskDto
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int ElapsedTime { get; set; }
        public bool TaskStatus { get; set; }
    }
}
