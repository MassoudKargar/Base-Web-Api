namespace Base.Domain.Users;
public class UserDatabase : IEntity
{
    public UserDatabase()
    {
        UserName = string.Empty;
        Password = string.Empty;
        ComputerName = string.Empty;
        Ip = string.Empty;
    }
    public UserDatabase(string userName, string password, string computerName, string ip) : this()
    {
        UserName = userName;
        Password = password;
        ComputerName = computerName;
        Ip = ip;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string ComputerName { get; set; }
    public string Ip { get; set; }
}