using Microsoft.AspNetCore.Identity;

namespace bislerium_blogs.Models
{
    public class CustomUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
