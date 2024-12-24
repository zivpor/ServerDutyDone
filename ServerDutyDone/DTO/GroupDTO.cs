namespace ServerDutyDone.DTO
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public int? GroupAdmin { get; set; }
        public string? GroupName { get; set; }
        public int? GroupType { get; set; }
        public GroupDTO() { }
        public GroupDTO(Models.Group group)
        {
            this.GroupAdmin = group.GroupAdmin;
            this.GroupName = group.GroupName;
            this.GroupType = group.GroupType;
        }

    }
}
