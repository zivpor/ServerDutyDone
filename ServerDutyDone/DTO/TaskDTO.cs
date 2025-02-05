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
        public TaskDTO() { }
        public TaskDTO(Models.Task task)
        {
            this.TaskName = task.TaskName;
            this.StatusId = task.StatusId;
            this.TaskId = task.TaskId;
            this.DueDay = task.DueDay;
        }
    }
}
