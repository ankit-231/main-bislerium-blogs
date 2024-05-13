using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Implementations;
using bislerium_blogs.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace bislerium_blogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly IAuthService _authenticationService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<CustomUser> userManager, IAuthService authService)
        {
            //_authenticationService = authenticationService;
            _userManager = userManager;
            _authService = authService;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequestPayload registerRequest)
        //{
        //    await _authenticationService.Register(registerRequest);
        //    return Ok();
        //}

        [HttpPost("register/user")]
        public async Task<IActionResult> Register(RegisterRequestPayload model)
        {

            return await _authService.Register(model, "user");
        }

        [HttpPost("register/admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterRequestPayload model)
        {
            //    System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            //    System.Diagnostics.Debug.WriteLine("ModelState.IsValid");
            //    //if (ModelState.IsValid)
            //    //{
            //    var user = new CustomUser
            //    {
            //        UserName = model.Email,
            //        Email = model.Email,
            //        FirstName = model.FirstName,
            //        LastName = model.LastName
            //    };

            //    var result = await _userManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded)
            //    {
            //        // User registration successful
            //        var createdUser = await _userManager.FindByIdAsync(user.Id);

            //        // Add role to the user
            //        var roleName = "Admin"; // Specify the role name you want to assign
            //        var roleResult = await _userManager.AddToRoleAsync(createdUser, roleName);

            //        if (roleResult.Succeeded)
            //        {
            //            return Ok("User registered successfully and role added.");
            //        }
            //        else
            //        {
            //            return BadRequest(roleResult.Errors);
            //        }

            //    }

            //    // If registration fails, return the error messages
            //    return BadRequest(result.Errors);
            //}

            // If model state is not valid, return a bad request response
            //return BadRequest(ModelState);
            return await _authService.Register(model, "admin");
            //return Ok("heheh");

        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _authService.GetUserProfile(userId);

        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdate updatedData)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _authService.UpdateUserProfile(userId, updatedData);

        }

        [HttpDelete("profile")]
        [Authorize]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _authService.DeleteUser(userId);

        }



        [HttpGet("testing")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Testing()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var message = new { loggedinuserid = user };
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user));
            return Ok(new { message = message, roles = roles });
        }

        [HttpGet("testing1")]
        public async Task<IActionResult> Testing1()
        {
            //get user with id "1"
            //var user = await _userManager.FindByIdAsync("5aa40f5b-21d3-4987-9a12-ffcea423e27f");
            //var user = await _userManager.
            //var users = await _userManager.Users.ToListAsync();
            //var users = await _userManager.Users.ToListAsync();
            //var users = await _userManager.Users.ToListAsync();
            var user = await _userManager.FindByIdAsync("6cb76597-37d4-44ec-b8e4-304267d2e4b6");

            //get first user from user table



            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine("User not found");
                return NotFound();
            }
            var roleName = "Admin";
            var result = await _userManager.AddToRoleAsync(user, roleName);
            //var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' added to user '{user.UserName}'");
            }
            else
            {
                return BadRequest(result.Errors);
            }


            return Ok(user);
        }

    }
}
