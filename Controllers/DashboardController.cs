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
using System.Text.Json.Serialization;
using System.Text.Json;

namespace bislerium_blogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DataContext _dataContext;
        //private readonly BlogService _blogService;


        public DashboardController(DataContext dataContext/*, BlogService blogService*/)
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
        [Route("Summary")]
        public async Task<IActionResult> GetAllBlogs()
        {
            // Get all blog posts
            //var allBlogPosts = await _dataContext.BlogModel.ToListAsync();

            var allBlogPosts = await _dataContext.BlogModel
                .Where(b => b.isCurrent)
                .Select(b => new BlogResponseDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    // Add additional fields here
                    Username = b.User.UserName,
                })
                .ToListAsync();

            //// Create JsonSerializerOptions with ReferenceHandler.Preserve
            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve
            //};

            //// Serialize the summary using JsonSerializerOptions
            //var json = JsonSerializer.Serialize(allBlogPosts, options);

            // Get all-time cumulative counts
            //int allTimeBlogPostCount = allBlogPosts.Count;
            //int allTimeUpvotes = allBlogPosts.Sum(post => post.Upvotes);
            //int allTimeDownvotes = allBlogPosts.Sum(post => post.Downvotes);
            //int allTimeComments = allBlogPosts.Sum(post => post.Comments.Count);


            return Ok(allBlogPosts);

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

        [HttpPut("UpdateBlog/{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogModel model)
        {
            // Retrieve the existing blog based on the provided ID
            var existingBlog = await _dataContext.BlogModel.FindAsync(id);


            if (existingBlog == null)
            {
                // If the blog does not exist, return an error response
                return NotFound("No blog found with the provided ID.");
            }

            // Check if blog belongs to logged in user
            if (existingBlog.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized("You are not authorized to update this blog.");
            }


            // set existing blog as not current
            existingBlog.isCurrent = false;

            await _dataContext.SaveChangesAsync();

            // Create a new blog entry with the updated content
            var newBlog = new BlogModel
            {
                Title = existingBlog.Title, // Keep the same title as the existing blog
                Content = model.Content, // Update the content with the provided value
                UploadedTimestamp = existingBlog.UploadedTimestamp, // Keep the same uploaded timestamp as the existing blog
                UpdatedTimestamp = DateTime.UtcNow, // Set the uploaded timestamp to the current time
                UserId = existingBlog.UserId, // Set the UserId to the existing blog's UserId
                ParentBlogId = existingBlog.ParentBlogId ?? id, // Set the ParentBlogId to the existing blog's ParentBlogId if not null, else use the ID from the request
                isCurrent = true
            };

            // Add the new blog entry to the database
            _dataContext.BlogModel.Add(newBlog);
            await _dataContext.SaveChangesAsync();

            // Return a success response
            return Ok("Blog updated successfully.");
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

            
            if (reactionDto.CommentId != null)
            {
                var parentComment = await _dataContext.CommentModel.FirstOrDefaultAsync(c => c.Id == reactionDto.CommentId);
                System.Diagnostics.Debug.WriteLine(parentComment);
                System.Diagnostics.Debug.WriteLine(reactionDto.CommentId);
                System.Diagnostics.Debug.WriteLine("dsaaadsadasdsa");
                if (parentComment == null)
                {
                    return NotFound("Comment not found.");
                }
            }
            var reaction = await _dataContext.ReactionModel.FirstOrDefaultAsync(r => r.BlogId == id && r.UserId == userId && r.CommentId == reactionDto.CommentId);


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
                    ReactionStatus = reactionDto.ReactionStatus,
                    CommentId = reactionDto.CommentId
                };
                _dataContext.ReactionModel.Add(reaction);
            }

            await _dataContext.SaveChangesAsync();
            //return Ok(reaction);
            return Ok("Reaction added successfully.");

        }

        // POST: api/Blogs/{id}/react
        //[HttpPost("Comment/{id}")]
        //[Authorize]  // Ensure the user is authenticated
        //public async Task<IActionResult> PostComment(int id, [FromBody] CommentModel commentDto)
        //{

        //    System.Diagnostics.Debug.WriteLine(commentDto);
        //    //System.Diagnostics.Debug.WriteLine("commentDtossasa");
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    System.Diagnostics.Debug.WriteLine(userId);
        //    ////if (userId == null)
        //    ////{
        //    ////    return Unauthorized("User is not logged in.");
        //    ////}
        //    if (commentDto.ParentCommentId != null)
        //    {
        //        var parentComment = await _dataContext.CommentModel.FirstOrDefaultAsync(c => c.Id == commentDto.ParentCommentId);
        //        System.Diagnostics.Debug.WriteLine(parentComment);
        //        System.Diagnostics.Debug.WriteLine(commentDto.ParentCommentId);
        //        System.Diagnostics.Debug.WriteLine("dsaaadsadasdsa");
        //        if (parentComment == null)
        //        {
        //            return NotFound("Comment not found.");
        //        }
        //    }
            


        //    //if (reaction != null)
        //    //{
        //    //    // Update existing reaction
        //    //    reaction.ReactionStatus = commentDto.ReactionStatus;
        //    //}
        //    //else
        //    // Create new reaction
        //    System.Diagnostics.Debug.WriteLine(commentDto);
        //    System.Diagnostics.Debug.WriteLine(commentDto.Content);
        //    System.Diagnostics.Debug.WriteLine(id);
        //    System.Diagnostics.Debug.WriteLine(userId);
        //    System.Diagnostics.Debug.WriteLine("dsadsadasdsa");
        //    CommentModel comment = new CommentModel
        //    {
        //        BlogId = id,
        //        UserId = userId,
        //        Content = commentDto.Content,
        //        ParentCommentId = commentDto.ParentCommentId

        //    };
        //    _dataContext.CommentModel.Add(comment);
        //    await _dataContext.SaveChangesAsync();
        //    //return Ok(reaction);
        //    return Ok("Comment added successfully.");
        //}
    }
}
