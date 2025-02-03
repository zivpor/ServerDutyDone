using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ServerDutyDone.Models;

namespace ServerDutyDone.ModelsBL
{
    public class ZivDBContext: DbContext

    {
        public User? GetUser(string email)
        {      
            return this.Users.Where(u => u.UserEmail == email)
                                .Include(u => u.UserTasks)
                                .ThenInclude(t => t.TaskComments)
                                .FirstOrDefault();
        }

        public Group? GetGroup(int groupId)
        {
            return this.Groups.Where(g => g.GroupId == groupId).FirstOrDefault();
        }
    }
}
