using bislerium_blogs.DTO;
using Microsoft.AspNetCore.Mvc;

namespace bislerium_blogs.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IActionResult> Register(RegisterRequestPayload registerPayload, string role);

        Task<IActionResult> GetUserProfile(string userId);

        Task<IActionResult> UpdateUserProfile(string userId, UserProfileUpdate updatedProfile);

        Task<IActionResult> DeleteUser(string userId);

    }
}
