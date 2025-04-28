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
        public List<Task> GetTasks(int userId)
        {
            User? u  = this.Users.Where(u => u.UserId == userId).FirstOrDefault();
            List<Group> groupsAsAdmin = this.Groups.Include(g=>g.Tasks).Where(g => g.GroupAdmin == userId).ToList();
            List<Group> groups = this.Groups.Include(g => g.Tasks).Where(g => g.Users.Contains(u)).ToList();


            List<Task> tasks = new List<Task>();

            foreach (Group g in groups)
            {
                foreach (Task t in g.Tasks)
                {
                    tasks.Add(t);
                }
            }

            foreach (Group g in groupsAsAdmin)
            {
                foreach (Task t in g.Tasks)
                {
                    tasks.Add(t);
                }
            }


            return tasks;
        }
        public List<User> GetUsers()
        {
            return this.Users.ToList();
        }
        public Models.User? GetUser1(int id)
        {
            return this.Users.Where(u => u.UserId == id)
                                .FirstOrDefault();
        }
    }
}
