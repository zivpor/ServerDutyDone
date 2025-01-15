using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerDutyDone.DTO;
using ServerDutyDone.Models;


namespace ServerDutyDone.Controllers
{
    [Route("api")]
    [ApiController]
    public class DutyDoneController : ControllerBase
    {

        // הגדרת הקישור למסד הנתונים
        private ZivDBContext context;

        //הגדרת סביבת העבודה
        private IWebHostEnvironment WebHostenvironment;


        // הגדרת פעולת בניית המחלקה 
        public DutyDoneController(ZivDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.WebHostenvironment = webHostEnvironment;
        }

        // הגדרת פעולת התחברות
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInfo loginInfo)

        {
            try
            {

                //LogOut any user that is already logged in
                HttpContext.Session.Clear();

                // קבלת פרטי המשתמש ממסד הנתונים
                Models.User? user = context.Users.FirstOrDefault(u => u.Email == loginInfo.Email);

                // בדיקה האם המשתמש קיים
                if (user == null)
                {
                    return NotFound();
                }
                if (user.UserPassword != loginInfo.Password)
                {
                    return Unauthorized();
                }
                // HttpContext.Session its an object that allows you to store temporary data for the current user.
                HttpContext.Session.SetString("LoggedInUser", user.Email);

                // create a new DTO.User object based on the existing user object.
                DTO.UserDTO DTO_User = new DTO.UserDTO(user);
                // החזרת פרטי המשתמש
                return Ok(DTO_User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] DTO.UserDTO user_dto)
        {
            if (user_dto == null)
            {
                return BadRequest("Invalid user data.");
            }

            // יצירת יוזר חדש בהתבסס על הקלט מהמשתמש
            Models.User modeluser = new Models.User
            {
                Username = user_dto.Username,
                Email = user_dto.Email,
                UserPassword = user_dto.UserPassword,

            };

            // בדיקת תקינות אימייל ייחודי
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user_dto.Email);
            if (existingUser != null)
            {
                return Conflict("User with this email already exists.");
            }

            // הוספת המשתמש למסד הנתונים
            context.Users.Add(modeluser);
            await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
            return Ok(modeluser.UserId);
        }
        //פעולה שמחזירה רשימה של הקבוצות שהמחובר לא מנהל
        [HttpGet("GetGroups")]
        public IActionResult GetGroups()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<Group> groups = context.Groups.Where(g => g.GroupAdmin != u.UserId).Include(g => g.Users).ToList();
                List<Group> finalGroups = new List<Group>();
                foreach (Group g in groups)
                {
                    if (g.Users.Where(uu => uu.UserId == u.UserId).FirstOrDefault() != null)
                        finalGroups.Add(g);
                }

                List<GroupDTO> dtoGroups = new List<GroupDTO>();
                foreach (var group in finalGroups)
                {
                    dtoGroups.Add(new GroupDTO(group));
                }

                return Ok(dtoGroups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //פעולה שמחזירה רשימה של הקבוצות שהמחובר  מנהל
        [HttpGet("GetManagerGroups")]
        public IActionResult GetManagerGroups()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not logged in");
                }

                List<Group> groups = context.Groups.Where(g => g.GroupAdmin == u.UserId).ToList();

                List<GroupDTO> dtoGroups = new List<GroupDTO>();
                foreach (var group in groups)
                {
                    dtoGroups.Add(new GroupDTO(group));
                }

                return Ok(dtoGroups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody] DTO.GroupDTO group_dto)
        {
            try
            {
                if (group_dto == null)
                {
                    return BadRequest("Invalid user data.");
                }

                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("LoggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                User? u = context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (u == null)
                {
                    return Unauthorized("User is not in the DB");
                }

                if (u.UserId != group_dto.GroupAdmin)
                {
                    return Unauthorized($"User ith id: {u.UserId} is trying to create group for user {group_dto.GroupAdmin}");
                }



                // יצירת קבוצה בהתבסס על הקלט מהמשתמש
                Models.Group modelgroup = new Models.Group
                {
                    GroupAdmin = group_dto.GroupAdmin,
                    GroupName = group_dto.GroupName,
                    GroupType = group_dto.GroupType,

                };




                // הוספת המשתמש למסד הנתונים
                context.Groups.Add(modelgroup);
                await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
                return Ok(modelgroup.GroupId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetBasicData")]
        public IActionResult GetBasicData()
        {
            try
            {

                List<GroupTypeDTO> groupTypes = new List<GroupTypeDTO>();
                foreach (var groupType in context.GroupTypes.ToList())
                    groupTypes.Add(new GroupTypeDTO(groupType));

                List<TaskTypeDTO> taskTypes = new List<TaskTypeDTO>();
                foreach (var ty in context.TaskTypes.ToList())
                    taskTypes.Add(new TaskTypeDTO(ty));

                List<TaskStatusDTO> statuses = new List<TaskStatusDTO>();
                foreach (var ts in context.TaskStatuses.ToList())
                    statuses.Add(new TaskStatusDTO(ts));

                AppBasicData data = new AppBasicData()
                {
                    GroupTypes = groupTypes,
                    TaskTypes = taskTypes,
                    TaskStatuses = statuses
                };


                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPost("AddUserToGroup")]
        //public async Task<IActionResult> AddUserToGroup([FromBody] DTO.GroupDTO group_dto, [FromBody] DTO.UserDTO user_dto)
        //{
        //    try
        //    {
                

                
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
    }
}
