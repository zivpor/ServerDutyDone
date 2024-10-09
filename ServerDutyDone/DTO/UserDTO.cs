namespace ServerDutyDone.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public UserDTO() { }
        public UserDTO(Models.User user)
        {
            this.Email = Email;
            this.Username = Username;
            this.UserPassword = UserPassword;
        }

    }
}
