using System.Collections.Generic;

public class User
{
    public string username { get; set; }
    public string password { get; set; }
    public List<string> oldPasswords { get; set; }
    public List<int> progress { get; set; }
}