using Microsoft.AspNetCore.Identity;

namespace LocalVista.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
}
