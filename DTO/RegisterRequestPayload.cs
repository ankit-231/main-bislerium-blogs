using bislerium_blogs.Models;

namespace bislerium_blogs.DTO
{
    public class RegisterRequestPayload : CustomUser
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
    }
}