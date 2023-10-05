namespace BackEnd.DTOs
{
    public class userDto
    {
   
        public int? userId { get; set; }
        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int? userRole { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
