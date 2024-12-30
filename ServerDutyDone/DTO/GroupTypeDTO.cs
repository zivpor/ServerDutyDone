namespace ServerDutyDone.DTO
{
    public class GroupTypeDTO
    {
        public int GroupTypeId { get; set; }
        public string? GroupTypeName { get; set; }
        public GroupTypeDTO() { }
        public GroupTypeDTO(Models.GroupType groupType)
        {
            this.GroupTypeName = groupType.GroupTypeName;
            this.GroupTypeId = groupType.GroupTypeId;
        }
    }
}
