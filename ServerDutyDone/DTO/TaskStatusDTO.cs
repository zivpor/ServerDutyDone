namespace ServerDutyDone.DTO
{
    public class TaskStatusDTO
    {
        public int TaskStatusId { get; set; }
        public string? TaskStatusName { get; set; }
        public TaskStatusDTO() { }
        public TaskStatusDTO(Models.TaskStatus taskStatus)
        {
            this.TaskStatusName = taskStatus.TypeName;
            this.TaskStatusId = taskStatus.StatusId;
        }
    }
}
