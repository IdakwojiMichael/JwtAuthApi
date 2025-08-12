namespace JwtAuthApi.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Auto-generated
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Still hashed
        public string Role { get; set; } = "User";
    }
}

