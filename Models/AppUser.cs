using Microsoft.AspNetCore.Identity;

namespace BookStore.Models;

public class AppUser : IdentityUser
{
    public bool IsActive { get; set; } = true;
}
