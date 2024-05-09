using Microsoft.AspNetCore.Identity;

namespace bislerium_blogs.Models
{
    public class CustomUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Role { get; set; }

        public ICollection<BlogModel>? Blogs { get; set; }

        public ICollection<CommentModel>? Comments { get; set; }

        public ICollection<ReactionModel>? Reactions { get; set; }

        
    }
}
