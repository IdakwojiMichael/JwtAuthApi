using JwtAuthApi.Models;
using JwtAuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _filePath = Path.Combine("Data", "users.json");
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;

            // Ensure Data folder & JSON file exist
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            if (!System.IO.File.Exists(_filePath))
                System.IO.File.WriteAllText(_filePath, "[]");
        }

        private List<User> LoadUsers()
        {
            var json = System.IO.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_filePath, json);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        [HttpPost("register")]
        public IActionResult Register(string username, string password, string role = "User")
        {
            var users = LoadUsers();

            if (users.Any(u => u.Username == username))
                return BadRequest("Username already exists.");

            var newUser = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Role = role
            };

            users.Add(newUser);
            SaveUsers(users);

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var users = LoadUsers();
            var hashedPassword = HashPassword(password);

            var user = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var token = _jwtTokenService.GenerateToken(user, user.Role);

            return Ok(new { token });
        }
    }
}
