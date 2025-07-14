public interface IProgressService
{
    int GetProgress(string username, int slot);
    bool SetProgress(string username, int slot, int level);
}