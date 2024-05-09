using bislerium_blogs.Data;
using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace bislerium_blogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BlogController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly BlogService _blogService;


        public BlogController(DataContext dataContext/*, BlogService blogService*/)
        {
            _dataContext = dataContext;
            //_blogService = blogService;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("This is your blog");
        //}

        //[HttpGet(Name = "AllBlogs")]
        [HttpGet]
        [Route("AllBlogs"), Authorize]
        public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        {
            //var blogs = new List<string>
            //{
            //    "Blog 1",
            //    "Blog 2",
            //    "Blog 3"
            //};
            //System.Diagnostics.Debug.WriteLine("User Claims:");

            //var User = HttpContext.User;
            //System.Diagnostics.Debug.WriteLine(User.FindFirstValue);

            //User.Claims.ToList().ForEach(c => System.Diagnostics.Debug.WriteLine($"Claim Type: {c.Type} - Claim Value: {c.Value}"));
            //var blogs = new List<BlogModel> {
            //    new BlogModel
            //    {
            //        Title = "Blog 1",
            //        Content = "This is the content of Blog 1",
            //        UploadedTimestamp = DateTime.Now
            //    }
            //};

            //bool isAdmin = false;
            //try
            //{
            //    isAdmin = await _blogContext.UserDetail.Where(item => item.UserId == CommonService.GetUserId(_httpContextAccessor.HttpContext)).Select(item => item.IsAdmin).FirstOrDefaultAsync();
            //}
            //catch (Exception ex) { }

            //List<DetailedBlogApplications> data = new List<DetailedBlogApplications>();

            //if (isAdmin)
            //{
            //    data = (from blog in _blogContext.BlogApplication
            //            join user in _blogContext.UserDetail on blog.UserId equals user.UserId
            //            where blog.IsDeleted != true
            //            select new DetailedBlogApplications
            //            {
            //                BlogId = blog.BlogId,
            //                BlogTitle = blog.BlogTitle,
            //                BlogDescription = blog.BlogDescription,
            //                CreatedOn = blog.CreatedOn,
            //                FullName = user.FirstName + " " + user.LastName,
            //                UserId = blog.UserId,
            //            }).ToList();
            //}
            //else
            //{
            //    data = (from blog in _blogContext.BlogApplication
            //            join user in _blogContext.UserDetail on blog.UserId equals user.UserId
            //            where blog.IsDeleted != true && blog.UserId == CommonService.GetUserId(_httpContextAccessor.HttpContext)
            //            select new DetailedBlogApplications
            //            {
            //                BlogId = blog.BlogId,
            //                BlogTitle = blog.BlogTitle,
            //                BlogDescription = blog.BlogDescription,
            //                CreatedOn = blog.CreatedOn,
            //                FullName = user.FirstName + " " + user.LastName,
            //                UserId = blog.UserId,
            //            }).ToList();
            //}


            //foreach (var blog in data)
            //{
            //    var images = await _blogContext.BlogImages
            //        .Where(img => img.BlogId == blog.BlogId && img.IsDeleted != true)
            //        .ToListAsync();

            //    List<BlogImageDetailed> detailedImages = images.Select(img => new BlogImageDetailed
            //    {
            //        ImageId = img.ImageId,
            //        BlogId = img.BlogId,
            //        ImagePath = img.ImagePath,
            //    }).ToList();

            //    Parallel.ForEach(detailedImages, img =>
            //    {
            //        img.ImageBytes = ReadLocalImageAsByteArray(img.ImagePath).Result;
            //    });

            //    blog.BlogImages = detailedImages;
            //}

            //return data.ToList();

            var blogs = await _dataContext.BlogModel.ToListAsync();


            return Ok(blogs);

            //return Ok();

            //return Ok("This is your blog");
        }

        //create post method for adding blog
        [HttpPost]
        [Route("AddBlog"), Authorize]
        public async Task<ActionResult<BlogModel>> AddBlog(BlogModel blog)
        {

            blog.UploadedTimestamp = DateTime.Now;
            blog.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogg = _dataContext.BlogModel.Add(blog);
            System.Diagnostics.Debug.WriteLine(blogg);
            System.Diagnostics.Debug.WriteLine(blog);

            System.Diagnostics.Debug.WriteLine("blogggggg");
            await _dataContext.SaveChangesAsync();
            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
            //return Ok(blog);
        }

        // Optional: GET by ID (to support CreatedAtAction)
        [HttpGet]
        [Route("GetBlog/{id}")]
        public async Task<ActionResult<BlogModel>> GetBlog(int id)
        {
            var blog = await _dataContext.BlogModel.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Blogs/{id}/react
        [HttpPost("React/{id}")]
        [Authorize]  // Ensure the user is authenticated
        public async Task<IActionResult> PostReaction(int id, [FromBody] ReactionModel reactionDto)
        {

            System.Diagnostics.Debug.WriteLine(reactionDto);
            //System.Diagnostics.Debug.WriteLine("reactionDtossasa");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine(userId);
            ////if (userId == null)
            ////{
            ////    return Unauthorized("User is not logged in.");
            ////}

            var reaction = await _dataContext.ReactionModel.FirstOrDefaultAsync(r => r.BlogId == id && r.UserId == userId);



            if (reaction != null)
            {
                // Update existing reaction
                reaction.ReactionStatus = reactionDto.ReactionStatus;
            }
            else
            {
                // Create new reaction
                System.Diagnostics.Debug.WriteLine(reactionDto);
                System.Diagnostics.Debug.WriteLine(reactionDto.ReactionStatus);
                System.Diagnostics.Debug.WriteLine(id);
                System.Diagnostics.Debug.WriteLine(userId);
                System.Diagnostics.Debug.WriteLine("dsadsadasdsa");

                reaction = new ReactionModel
                {
                    BlogId = id,
                    UserId = userId,
                    ReactionStatus = reactionDto.ReactionStatus
                };
                _dataContext.ReactionModel.Add(reaction);
            }

            await _dataContext.SaveChangesAsync();
            //return Ok(reaction);
            return Ok("Reaction added successfully.");

        }
    }
}
