namespace Project.Models.DTO
{
    public class AddUserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}
