using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? PictureUrl { get; set; }

    }
}
