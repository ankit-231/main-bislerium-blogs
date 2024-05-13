using Google.Protobuf.WellKnownTypes;

namespace bislerium_blogs.DTO
{
    // UserProfile.cs
    public class UserProfile
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Role { get; set; }
        // Add any other properties you want to include in the profile
    }
}