
namespace Services
{
    public class ProgressService
    {
        private UserDataService userService;

        public ProgressService(UserDataService userService)
        {
            this.userService = userService;
        }

        public int GetProgress(string username, int slot)
        {
            var user = userService.GetUser(username);
            if (user == null || slot < 0 || slot >= user.progress.Count) return 0;
            return user.progress[slot];
        }

        public bool SetProgress(string username, int slot, int level)
        {
            var user = userService.GetUser(username);
            if (user == null || slot < 0 || slot >= user.progress.Count) return false;
            user.progress[slot] = level;
            return userService.UpdateUser(user);
        }
    }
}