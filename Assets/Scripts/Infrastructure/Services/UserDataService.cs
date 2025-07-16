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

        // Cargar usuarios desde disco
        private void Load()
        {
            if (File.Exists(filePath))
                users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(filePath));
            else
                users = new List<User>();
        }

        // Guardar usuarios a disco
        private void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(users, Formatting.Indented));
        }

        // Obtener usuario completo por username
        public User GetUser(string username) => users.Find(u => u.username == username);

        // Obtener todos los usuarios
        public List<User> GetAllUsers() => users;

        // A�adir usuario nuevo
        public bool AddUser(User user)
        {
            if (users.Exists(u => u.username == user.username)) return false;
            users.Add(user);
            Save();
            return true;
        }

        // Cambiar contraseña (y registrar la anterior)
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
        public bool ChangeUserName(string oldUsername, string newUsername)
        {
            // 1. Validar que el nuevo nombre no exista ya
            if (users.Exists(u => u.username == newUsername))
                return false;
            // 2. Buscar el usuario con el nombre viejo
            var user = users.Find(u => u.username == oldUsername);
            if (user == null)
                return false;
            // 3. Actualizar solo el campo username
            user.username = newUsername;
            Save();
            return true;
        }
        // Validar password
        public bool ValidatePassword(string username, string password)
        {
            var user = GetUser(username);
            return user != null && user.password == password;
        }

        // Actualizar todos los datos del usuario y guardar
        public bool UpdateUser(User user)
        {
            var idx = users.FindIndex(u => u.username == user.username);
            if (idx == -1) return false;
            users[idx] = user;
            Save();
            return true;
        }

        // Actualizar progreso de un slot
        public bool UpdateProgress(string username, int slot, int nivel)
        {
            var user = GetUser(username);
            if (user == null || user.progress == null || slot < 0 || slot >= user.progress.Count) return false;
            user.progress[slot] = nivel;
            Save();
            return true;
        }

        // Recargar los datos desde disco por si han cambiado externamente
        public void Reload()
        {
            Load();
        }

        // M�todo est�tico para un acceso r�pido desde SessionManager (opcional)
        public static User LoadUserFromDisk(string username)
        {
            if (File.Exists(filePath))
            {
                var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(filePath));
                return users.Find(u => u.username == username);
            }
            return null;
        }
    }
}