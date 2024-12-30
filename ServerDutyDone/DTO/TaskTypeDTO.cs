namespace ServerDutyDone.DTO
{
    public class TaskTypeDTO
    {
        public int TaskTypeId { get; set; }
        public string? TaskTypeName { get; set; }
        public TaskTypeDTO() { }
        public TaskTypeDTO(Models.TaskType taskType)
        {
            this.TaskTypeName = taskType.TypeName;
            this.TaskTypeId = taskType.TypeId;
        }
    }
}
