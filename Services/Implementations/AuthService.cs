using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace bislerium_blogs.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AuthService(SignInManager<CustomUser> signInManager, UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        //public async Task Register(string firstName, string lastName, string email, string password)
        public async Task Register(RegisterRequestPayload registerPayload)
        {
            var newUser = new CustomUser { FirstName = registerPayload.FirstName, LastName = registerPayload.LastName, UserName = registerPayload.Email, Email = registerPayload.Email };
            var result = await _userManager.CreateAsync(newUser, registerPayload.Password);
            ValidateIdentityResult(result);

            await _userManager.AddToRoleAsync(newUser, "User");
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            //var token = ToUrlSafeBase64(emailConfirmationToken);
            //await _emailService.SendEmailConfirmationEmailAsync(firstName, lastName, newUser.Id, email, token);
        }

        private void ValidateIdentityResult(IdentityResult result)
        {
            if (result.Succeeded) return;
            var errors = result.Errors.Select(x => x.Description);
            throw new Exception(string.Join('\n', errors));
        }
    }
}
