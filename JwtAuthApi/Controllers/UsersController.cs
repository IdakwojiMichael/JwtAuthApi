using JwtAuthApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // All endpoints require Admin
    public class UsersController : ControllerBase
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.json");

        // Load users from file
        private List<User> LoadUsers()
        {
            if (!System.IO.File.Exists(_filePath))
                return new List<User>();

            var json = System.IO.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        // Save users to file
        private void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_filePath, json);
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = LoadUsers();
            var safeUsers = users.Select(u => new
            {
                u.Id,
                u.Username,
                u.Role
            });
            return Ok(safeUsers);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Role
            });
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            users.Remove(user);
            SaveUsers(users);

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
