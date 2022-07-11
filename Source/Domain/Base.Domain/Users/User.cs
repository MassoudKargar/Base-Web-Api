namespace Base.Domain.Users;

public class User : IEntity
{
    public User()
    {
        FullName = String.Empty;
    }

    public User(long personId, long userId, string fullName) : this()
    {
        PersonId = personId;
        UserId = userId;
        FullName = fullName;
    }

    public long PersonId { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; }
}
