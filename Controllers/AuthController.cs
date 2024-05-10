using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bislerium_blogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly UserManager<CustomUser> _userManager;

        public AuthController(IAuthService authenticationService, UserManager<CustomUser> userManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequestPayload registerRequest)
        //{
        //    await _authenticationService.Register(registerRequest);
        //    return Ok();
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestPayload model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // User registration successful
                    return Ok("User registered successfully.");
                }

                // If registration fails, return the error messages
                return BadRequest(result.Errors);
            }

            // If model state is not valid, return a bad request response
            return BadRequest(ModelState);
        }
    }
}
