using bislerium_blogs.Data;
using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Implementations;
using bislerium_blogs.Services.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace bislerium_blogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BlogController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IBlogService _blogService;
        //private readonly BlogService _blogService;


        public BlogController(DataContext dataContext, IBlogService blogService)
        {
            _dataContext = dataContext;
            _blogService = blogService;
            //_blogService = blogService;
        }

        [HttpGet]
        [Route("AllBlogs")]
        //public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        public async Task<IActionResult> GetAllBlogs([FromQuery] string? sortBy, [FromQuery] PaginationFilter filter)
        {
            System.Console.WriteLine(sortBy);
            System.Console.WriteLine("isisiisisisi");
            System.Console.WriteLine(filter);
            Console.WriteLine(filter.PageNumber);
            Console.WriteLine(filter.PageSize);
            //return await _blogService.GetAllBlogs(_sortBy: sortBy, filter: filter);
            return await _blogService.GetPaginatedBlogs(_sortBy: sortBy, filter: filter);

        }

        [HttpGet]
        [Route("TopTenBlogs")]
        //public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        public async Task<IActionResult> GetTopTenBlogs([FromQuery] string? timeType)
        {
            var sortBy = "popular";
            System.Console.WriteLine(sortBy);
            System.Console.WriteLine("isisiisisisi");
            //System.Console.WriteLine(filter);
            //Console.WriteLine(filter.PageNumber);
            //Console.WriteLine(filter.PageSize);
            //return await _blogService.GetAllBlogs(_sortBy: sortBy, filter: filter);
            return await _blogService.GetPaginatedBlogs(_sortBy: sortBy, total: 10, timeType: timeType);

        }

        [HttpGet]
        [Route("TopTenAuthors")]
        //public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        public async Task<IActionResult> GetTopTenAuthors([FromQuery] string? timeType)
        {
            var sortBy = "popular";
            System.Console.WriteLine(sortBy);
            System.Console.WriteLine("isisiisisisi");
            //System.Console.WriteLine(filter);
            //Console.WriteLine(filter.PageNumber);
            //Console.WriteLine(filter.PageSize);
            //return await _blogService.GetAllBlogs(_sortBy: sortBy, filter: filter);
            return await _blogService.GetTopTenAuthors(timeType: timeType);

        }

        [HttpGet]
        [Route("AllBlogsPaginated")]
        //public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        public async Task<IActionResult> GetAllBlogsPaginated([FromQuery] string? sortBy, [FromQuery] PaginationFilter filter)
        {
            System.Console.WriteLine(sortBy);
            System.Console.WriteLine("isisiisisisi");
            System.Console.WriteLine(filter);
            Console.WriteLine(filter.PageNumber);
            Console.WriteLine(filter.PageSize);
            return await _blogService.GetAllBlogs(_sortBy: sortBy, filter: filter);
            //return await _blogService.GetPaginatedBlogs(_sortBy: sortBy, filter: filter);

        }

        [HttpGet]
        [Route("MyBlogs")]
        [Authorize]
        //public async Task<ActionResult<List<BlogModel>>> GetAllBlogs()
        public async Task<IActionResult> GetMyBlogs()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            return await _blogService.GetAllBlogs(userId);
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

            // Create a new history log entry
            var historyLog = new HistoryLog
            {
                OldContent = existingBlog.Content,
                NewContent = model.Content,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Timestamp = DateTime.UtcNow,
                IsBlog = true,
                EntityId = existingBlog.Id,
            };

            // Update the properties of the existing blog with the new values
            existingBlog.Title = model.Title;
            existingBlog.Content = model.Content;
            existingBlog.UpdatedTimestamp = DateTime.UtcNow;
            existingBlog.UploadedTimestamp = existingBlog.UploadedTimestamp;

            // Save the changes to the database
            await _dataContext.SaveChangesAsync();

            // Save the history log entry
            _dataContext.HistoryLog.Add(historyLog);
            await _dataContext.SaveChangesAsync();


            //await _dataContext.SaveChangesAsync();

            return Ok("Blog updated successfully.");
        }

        [HttpDelete("DeleteBlog/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
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
                return Unauthorized("You are not authorized to delete this blog.");
            }

            //delete existingBlog with await
            _dataContext.BlogModel.Remove(existingBlog);

            await _dataContext.SaveChangesAsync();

            //var result = await _dataContext.BlogModel.Remove(existingBlog);

            return Ok("Blog deleted successfully.");
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

        [HttpGet]
        [Route("GetBlogHistory/{id}")]
        [Authorize]
        public async Task<IActionResult> GetBlogHistory(int id)
        {
            var existingBlog = await _dataContext.BlogModel.FindAsync(id);


            if (existingBlog == null)
            {
                // If the blog does not exist, return an error response
                return NotFound("No blog found with the provided ID.");
            }

            // Check if blog belongs to logged in user
            if (existingBlog.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized("You are not authorized to get history of this blog.");
            }

            return await _blogService.GetHistory(id, true);

        }

        [HttpGet]
        [Route("GetCommentHistory/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCommentHistory(int id)
        {
            var existingComment = await _dataContext.CommentModel.FindAsync(id);


            if (existingComment == null)
            {
                // If the comment does not exist, return an error response
                return NotFound("No comment found with the provided ID.");
            }

            // Check if blog belongs to logged in user
            if (existingComment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized("You are not authorized to get history of this comment.");
            }

            return await _blogService.GetHistory(id, false);

        }


        [HttpPost("React/{id}")]
        [Authorize]  // Ensure the user is authenticated
        public async Task<IActionResult> PostReaction(int id, [FromBody] ReactionModel reactionDto)
        {

            System.Diagnostics.Debug.WriteLine(reactionDto);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine(userId);


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
        [HttpPost("Comment/{id}")]
        [Authorize]  // Ensure the user is authenticated
        public async Task<IActionResult> PostComment(int id, [FromBody] CommentModel commentDto)
        {

            System.Diagnostics.Debug.WriteLine(commentDto);
            //System.Diagnostics.Debug.WriteLine("commentDtossasa");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine(userId);
            ////if (userId == null)
            ////{
            ////    return Unauthorized("User is not logged in.");
            ////}
            if (commentDto.ParentCommentId != null)
            {
                var parentComment = await _dataContext.CommentModel.FirstOrDefaultAsync(c => c.Id == commentDto.ParentCommentId);
                System.Diagnostics.Debug.WriteLine(parentComment);
                System.Diagnostics.Debug.WriteLine(commentDto.ParentCommentId);
                System.Diagnostics.Debug.WriteLine("dsaaadsadasdsa");
                if (parentComment == null)
                {
                    return NotFound("Comment not found.");
                }
            }

            //Get Blog blog from id
            var blog = await _dataContext.BlogModel.FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound("Blog not found.");
            }
            var blogIdToSave = blog.Id;
            //if blog has parent blog make blog = its parent
            System.Diagnostics.Debug.WriteLine(blog.ParentBlogId);
            System.Diagnostics.Debug.WriteLine(blog.Id);
            System.Diagnostics.Debug.WriteLine("blog.ParentBlogId");


            if (blog.ParentBlogId != null)
            {
                //blog = await _dataContext.BlogModel.FirstOrDefaultAsync(b => b.Id == blog.ParentBlogId);
                blogIdToSave = (int)blog.ParentBlogId;
            }


            //if (reaction != null)
            //{
            //    // Update existing reaction
            //    reaction.ReactionStatus = commentDto.ReactionStatus;
            //}
            //else
            // Create new reaction
            System.Diagnostics.Debug.WriteLine(commentDto);
            System.Diagnostics.Debug.WriteLine(commentDto.Content);
            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine(userId);
            System.Diagnostics.Debug.WriteLine("dsadsadasdsa");
            CommentModel comment = new CommentModel
            {
                BlogId = blogIdToSave,
                UserId = userId,
                Content = commentDto.Content,
                ParentCommentId = commentDto.ParentCommentId

            };
            _dataContext.CommentModel.Add(comment);
            await _dataContext.SaveChangesAsync();
            //return Ok(reaction);
            return Ok("Comment added successfully.");
        }

        [HttpPut("UpdateComment/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentModel model)
        {
            // Retrieve the existing comment based on the provided ID
            var existingComment = await _dataContext.CommentModel.FindAsync(id);

            if (existingComment == null)
            {
                // If the comment does not exist, return an error response
                return NotFound("No comment found with the provided ID.");
            }

            // Check if the comment belongs to the logged-in user
            if (existingComment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized("You are not authorized to update this comment.");
            }

            // Create a history log entry for the update
            var historyLog = new HistoryLog
            {
                OldContent = existingComment.Content,
                NewContent = model.Content,
                UserId = existingComment.UserId,
                IsBlog = false, // Set IsBlog to false for comment updates,
                EntityId = existingComment.Id,
                Timestamp = DateTime.UtcNow
            };

            // Update the existing comment with the new content
            existingComment.Content = model.Content;

            // Add the history log entry to the database
            _dataContext.HistoryLog.Add(historyLog);

            // Save changes to the database
            await _dataContext.SaveChangesAsync();

            // Return a success response
            return Ok("Comment updated successfully.");
        }


    }
}
