using Infrastructure.Entities;

namespace Api.Models;

public class UserResponse : User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public UserResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Username = user.Username;
    }
}