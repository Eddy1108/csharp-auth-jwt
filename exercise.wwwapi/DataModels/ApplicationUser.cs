using workshop.webapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace workshop.webapi.DataModels;

public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}