using System.Collections.Generic;

public interface IUserService
{
    User GetUser(string username);
    bool AddUser(User user);
    bool UpdateUser(User user);
    bool ChangePassword(string username, string newPassword);
    bool ValidatePassword(string username, string password);
    List<User> GetAllUsers();
}