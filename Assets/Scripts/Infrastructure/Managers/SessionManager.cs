using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.Managers
{
    public static class SessionManager
    {
        public static string CurrentUser { get; private set; }
        public static User currentUser { get; private set; }

        public static void Login(string username)
        {
            CurrentUser = username;
            currentUser = UserDataService.LoadUserFromDisk(username);
        }

        public static void Logout()
        {
            CurrentUser = null;
            currentUser = null;
        }
    }
}
