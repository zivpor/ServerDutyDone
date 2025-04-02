namespace ServerDutyDone.DTO
{
    public class TaskDTO
    {
        public int TaskId { get; set; }

        public int? TaskType { get; set; }

        public DateOnly? DueDay { get; set; }

        public int? UserId { get; set; }
        public string? TaskName { get; set; }

        public int? GroupId { get; set; }

        public int? StatusId { get; set; }
        public string? TaskDescription { get; set; }
        public string? TaskUpdate { get; set; }
        public TaskDTO() { }
        public TaskDTO(Models.Task task)
        {
            this.TaskName = task.TaskName;
            this.StatusId = task.StatusId;
            this.TaskId = task.TaskId;
            this.DueDay = task.DueDay;
            this.TaskType = task.TaskType;
            this.UserId = task.UserId;
            this.GroupId = task.GroupId;
            this.TaskDescription = task.TaskDescription;
            this.TaskUpdate = task.TaskUpdate;
        }
        public Models.Task GetModel()
        {
            Models.Task task = new Models.Task()
            {
                TaskId = this.TaskId,
                TaskType = this.TaskType,
                DueDay = this.DueDay,
                UserId = this.UserId,
                TaskName = this.TaskName,
                GroupId = this.GroupId,
                StatusId = this.StatusId,
                TaskDescription= this.TaskDescription,
                TaskUpdate = this.TaskUpdate
            };
            return task;
        }
    }
}
