using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace bislerium_blogs.Services.Implementations
{
    public class AuthService : IAuthService
    {
        //private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IEmailService _emailService;

        public AuthService(/*SignInManager<CustomUser> signInManager, */UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager/*, IEmailService emailService*/)
        {
            //_signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            //_emailService = emailService;
        }

        //public async Task Register(string firstName, string lastName, string email, string password)
        //public async Task Register(RegisterRequestPayload model)
        //{
        //    //var newUser = new CustomUser { FirstName = registerPayload.FirstName, LastName = registerPayload.LastName, UserName = registerPayload.Email, Email = registerPayload.Email };
        //    //var result = await _userManager.CreateAsync(newUser, registerPayload.Password);
        //    //ValidateIdentityResult(result);

        //    //await _userManager.AddToRoleAsync(newUser, "User");

        //    //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        //    //var token = ToUrlSafeBase64(emailConfirmationToken);
        //    //await _emailService.SendEmailConfirmationEmailAsync(firstName, lastName, newUser.Id, email, token);

        //    if (ModelState.IsValid)
        //    {
        //        var user = new CustomUser
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName
        //        };

        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            // User registration successful
        //            var createdUser = await _userManager.FindByIdAsync(user.Id);

        //            // Add role to the user
        //            var roleName = "user"; // Specify the role name you want to assign
        //            var roleResult = await _userManager.AddToRoleAsync(createdUser, roleName);

        //            if (roleResult.Succeeded)
        //            {
        //                return Ok("User registered successfully and role added.");
        //            }
        //            else
        //            {
        //                return BadRequest(roleResult.Errors);
        //            }

        //            // User registration successful
        //            return Ok("User registered successfully.");
        //        }

        //        // If registration fails, return the error messages
        //        return BadRequest(result.Errors);
        //    }

        //    // If model state is not valid, return a bad request response
        //    return BadRequest(ModelState);
        //}
        public async Task<IActionResult> Register(RegisterRequestPayload model, string role)
        {
            //System.Console.WriteLine(model.Email);
            //System.Console.WriteLine(role);
            //System.Console.WriteLine("aaaaaaaaaa");

            if (model == null)
            {
                return new BadRequestObjectResult("Registration payload is null.");
            }

            // Check if the user already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new BadRequestObjectResult("User with this email already exists.");
            }

            // Create a new user object
            var newUser = new CustomUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            // Attempt to create the user
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }


            // Assign the 'user' role to the new user
            await _userManager.AddToRoleAsync(newUser, role);

            // Registration successful
            return new OkObjectResult("User registered successfully.");
        }
        private void ValidateIdentityResult(IdentityResult result)
        {
            if (result.Succeeded) return;
            var errors = result.Errors.Select(x => x.Description);
            throw new Exception(string.Join('\n', errors));
        }

        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            var role = await _userManager.GetRolesAsync(user);

            UserProfile profile = new()
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role.ToList()
            };
            return new OkObjectResult(profile);
        }

        public async Task<IActionResult> UpdateUserProfile(string userId, UserProfileUpdate updatedProfile)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            user.FirstName = updatedProfile.FirstName;
            user.LastName = updatedProfile.LastName;
            // Update other properties as needed

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new OkObjectResult("User profile updated successfully");
            }
            else
            {
                return new BadRequestObjectResult(result.Errors);
            }
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return new OkObjectResult("User deleted successfully");
            }
            else
            {
                return new BadRequestObjectResult(result.Errors);
            }
        }
    }
}
