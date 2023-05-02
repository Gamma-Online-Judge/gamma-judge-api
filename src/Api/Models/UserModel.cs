using Infrastructure.Entities;

namespace Api.Models;

public class UserResponse
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public UserResponse(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Username = user.Username;
    }
}