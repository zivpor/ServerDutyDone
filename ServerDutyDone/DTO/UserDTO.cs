namespace ServerDutyDone.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
        public string ProfileImagePath { get; set; }
        
        public UserDTO() { }
        public UserDTO(Models.User user)
        {
            this.Email = user.Email;
            this.Username = user.Username;
            this.UserPassword = user.UserPassword;
            this.UserId = user.UserId;
            this.IsAdmin = user.IsAdmin;
            this.IsBlocked = user.IsBlocked;
        }
        public Models.User GetModel()
        {
            return new Models.User()
            {
                UserId = this.UserId,
                Username = this.Username,
                UserPassword = this.UserPassword,
                Email = this.Email,
                IsAdmin = this.IsAdmin
            };
        }

    }
}
