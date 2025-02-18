using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerDutyDone.DTO;
using ServerDutyDone.Models;
//using ServerDutyDone.ModelsBL;


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
                DTO_User.ProfileImagePath = GetProfileImageVirtualPath(user.UserId);
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
        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Models.User? user = context.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (user == null)
            {
                return Unauthorized("User is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.WebHostenvironment.WebRootPath}\\profileImages\\{user.UserId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }

            DTO.UserDTO dtoUser = new DTO.UserDTO(user);
            dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
            return Ok(dtoUser);
        }
        //this function gets a file stream and check if it is an image
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

        //this function check which profile image exist and return the virtual path of it.
        //if it does not exist it returns the default profile image virtual path
        private string GetProfileImageVirtualPath(int userId)
        {
            string virtualPath = $"/profileImages/{userId}";
            string path = $"{this.WebHostenvironment.WebRootPath}\\profileImages\\{userId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.WebHostenvironment.WebRootPath}\\profileImages\\{userId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/profileImages/default.png";
                }
            }

            return virtualPath;
        }
        [HttpPost("UploadGroupProfileImage")]
        public async Task<IActionResult> UploadGroupProfileImageAsync(IFormFile file, [FromQuery] int groupid)
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 

            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (groupid == null)
            {
                return Unauthorized("group is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.WebHostenvironment.WebRootPath}\\profileImages\\{groupid}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }
            Group g = context.Groups.Where(gg => gg.GroupId == groupid).FirstOrDefault();
            DTO.GroupDTO dtoGroup = new DTO.GroupDTO(g);
            dtoGroup.GroupProfileImagePath = GetGroupProfileImageVirtualPath(dtoGroup.GroupId);
            return Ok(dtoGroup);
        }

        //this function check which profile image exist and return the virtual path of it.
        //if it does not exist it returns the default profile image virtual path
        private string GetGroupProfileImageVirtualPath(int groupId)
        {
            string virtualPath = $"/groupProfileImages/{groupId}";
            string path = $"{this.WebHostenvironment.WebRootPath}\\groupProfileImages\\{groupId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.WebHostenvironment.WebRootPath}\\groupProfileImages\\{groupId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/groupProfileImages/default.png";
                }
            }

            return virtualPath;
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] DTO.TaskDTO task_dto)
        {
            try
            {
                if (task_dto == null)
                {
                    return BadRequest("Invalid task data.");
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

                if (u.UserId != task_dto.UserId)
                {
                    return Unauthorized($"User ith id: {u.UserId} is trying to add task for user {task_dto.UserId}");
                }

                // יצירת קבוצה בהתבסס על הקלט מהמשתמש
                Models.Task modeltask = task_dto.GetModel();




                // הוספת המשתמש למסד הנתונים
                context.Tasks.Add(modeltask);
                context.SaveChanges(); // שמירת השינויים במסד הנתונים
                return Ok(modeltask.TaskId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            //this function gets a file stream and check if it is an image

            //[HttpGet("GetTasks")]
            //public IActionResult GetTasks()
            //{
            //    try
            //    {
            //        //Check if who is logged in
            //        string? userEmail = HttpContext.Session.GetString("LoggedInUser");
            //        if (string.IsNullOrEmpty(userEmail))
            //        {
            //            return Unauthorized("User is not logged in");
            //        }

            //        User? u = context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            //        if (u == null)
            //        {
            //            return Unauthorized("User is not logged in");
            //        }

            //        List<Task> tasks = context.Groups.Where(t => t.GroupAdmin != u.UserId).Include(g => g.Users).ToList();
            //        List<Group> finalGroups = new List<Group>();
            //        foreach (Group g in groups)
            //        {
            //            if (g.Users.Where(uu => uu.UserId == u.UserId).FirstOrDefault() != null)
            //                finalGroups.Add(g);
            //        }

            //        List<GroupDTO> dtoGroups = new List<GroupDTO>();
            //        foreach (var group in finalGroups)
            //        {
            //            dtoGroups.Add(new GroupDTO(group));
            //        }

            //        return Ok(dtoGroups);
            //    }
            //    catch (Exception ex)
            //    {
            //        return BadRequest(ex.Message);
            //    }
            //}
         [HttpPost("GetGroupTasks")]
        public IActionResult GetGroupTasks([FromBody] GroupDTO groupDTO)
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

                List<Models.Task> tasks = context.Tasks.Where(t => t.GroupId == groupDTO.GroupId).ToList();
                List<DTO.TaskDTO> list = new List<TaskDTO>();
                foreach(Models.Task task in tasks)
                {
                    list.Add(new TaskDTO(task));
                }


                

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }

