using System.Text.Json;
using JwtAuthApi.Models;

namespace JwtAuthApi.Repositories
{
    public class UserRepository
    {
        private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");

        public UserRepository()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<User> GetAllUsers()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public void SaveAllUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void AddUser(User user)
        {
            var users = GetAllUsers();
            users.Add(user);
            SaveAllUsers(users);
        }

        public User? GetUserByUsername(string username)
        {
            return GetAllUsers().FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
