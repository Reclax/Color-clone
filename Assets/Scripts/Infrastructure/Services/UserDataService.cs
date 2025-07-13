using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


namespace Services
{
    public class UserDataService
    {
        private static string fileName = "users.json";
        private static string filePath => Path.Combine(Application.dataPath, "Resources", fileName);
        private List<User> users;

        public UserDataService()
        {
            Load();
        }

        private void Load()
        {
            if (File.Exists(filePath))
                users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(filePath));
            else
                users = new List<User>();
        }

        private void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(users, Formatting.Indented));
        }

        public User GetUser(string username) => users.Find(u => u.username == username);

        public bool AddUser(User user)
        {
            if (users.Exists(u => u.username == user.username)) return false;
            users.Add(user);
            Save();
            return true;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            var user = GetUser(username);
            if (user == null || user.password == newPassword || user.oldPasswords.Contains(newPassword))
                return false;
            user.oldPasswords.Add(user.password);
            user.password = newPassword;
            Save();
            return true;
        }

        public bool ValidatePassword(string username, string password)
        {
            var user = GetUser(username);
            return user != null && user.password == password;
        }

        public List<User> GetAllUsers() => users;

        public bool UpdateUser(User user)
        {
            var idx = users.FindIndex(u => u.username == user.username);
            if (idx == -1) return false;
            users[idx] = user;
            Save();
            return true;
        }
    }
}