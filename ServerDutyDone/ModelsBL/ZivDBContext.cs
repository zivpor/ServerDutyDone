using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ServerDutyDone.DTO;


namespace ServerDutyDone.Models
{
    public partial class ZivDBContext: DbContext

    {
        public User? GetUser(string email)
        {
            return this.Users.FirstOrDefault(u => u.Email == email);
                                
        }

        public Group? GetGroup(int groupId)
        {
            return this.Groups.FirstOrDefault(g => g.GroupId == groupId);
        }
    }
}
