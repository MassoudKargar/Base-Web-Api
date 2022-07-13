namespace Base.Domain.Users;
public class UserDto : BaseDto<UserDto, UserDatabase>
{
    public UserDto()
    {
        UserName = string.Empty;
        Password = string.Empty;
    }
    public UserDto(string userName, string password) : this()
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
}