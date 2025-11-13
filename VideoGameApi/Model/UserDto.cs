namespace VideoGameApi.Model
{
    public class UserRegDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

    }
    public class UserLoginDto
    {
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
   

    }
}