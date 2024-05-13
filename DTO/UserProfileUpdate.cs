using Google.Protobuf.WellKnownTypes;

namespace bislerium_blogs.DTO
{
    // UserProfile.cs
    public class UserProfileUpdate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Add any other properties you want to include in the profile
    }
}