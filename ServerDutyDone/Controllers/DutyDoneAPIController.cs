using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerDutyDone.Models;


namespace ServerDutyDone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DutyDoneAPIController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private ZivDBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public DutyDoneAPIController(ZivDBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] DTO.UserDTO userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User modelsUser = new User()
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    UserPassword = userDto.UserPassword
                    
                };

                context.Users.Add(modelsUser);
                context.SaveChanges();

                //User was added!
                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
