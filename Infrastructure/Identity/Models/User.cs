using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models;

public class User : IdentityUser
{
    public string? FullName { get; private set; }

    public User() { }

    public User(string fullName) => FullName = fullName;

    public void UpdateFullName(string fullName) => FullName = fullName;
}
