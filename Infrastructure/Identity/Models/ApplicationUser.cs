using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; private set; }
    public ApplicationUser(string email, string name)
    {
        UserName = email;
        Email = email;
        Name = name;
    }
}
