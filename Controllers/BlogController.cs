using bislerium_blogs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bislerium_blogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("This is your blog");
        //}

        [HttpGet]
        public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        {
            //var blogs = new List<string>
            //{
            //    "Blog 1",
            //    "Blog 2",
            //    "Blog 3"
            //};
            System.Diagnostics.Debug.WriteLine("User Claims:");
            User.Claims.ToList().ForEach(c => System.Diagnostics.Debug.WriteLine($"Claim Type: {c.Type} - Claim Value: {c.Value}"));
            var blogs = new List<BlogModel> { 
                new BlogModel
                {
                    Title = "Blog 1",
                    Content = "This is the content of Blog 1",
                    UploadedTimestamp = DateTime.Now
                }
            };
            return Ok(blogs);

            //return Ok("This is your blog");
        }
    }
}
