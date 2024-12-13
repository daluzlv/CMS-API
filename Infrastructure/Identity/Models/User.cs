using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models;

public class User : IdentityUser
{
    public string FullName { get; set; }
}
