using bislerium_blogs.Data;
using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Implementations;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        //private readonly BlogService _blogService;


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
        [Route("AllBlogs")]
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

            var blogs = await _dataContext.BlogModel
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

            // Save the changes to the database
            await _dataContext.SaveChangesAsync();

            // Save the history log entry
            _dataContext.HistoryLog.Add(historyLog);
            await _dataContext.SaveChangesAsync();


            // set existing blog as not current
            //existingBlog.isCurrent = false;

            //await _dataContext.SaveChangesAsync();

            //// Create a new blog entry with the updated content
            //var newBlog = new BlogModel
            //{
            //    Title = existingBlog.Title, // Keep the same title as the existing blog
            //    Content = model.Content, // Update the content with the provided value
            //    UploadedTimestamp = existingBlog.UploadedTimestamp, // Keep the same uploaded timestamp as the existing blog
            //    UpdatedTimestamp = DateTime.UtcNow, // Set the uploaded timestamp to the current time
            //    UserId = existingBlog.UserId, // Set the UserId to the existing blog's UserId
            //    ParentBlogId = existingBlog.ParentBlogId ?? id, // Set the ParentBlogId to the existing blog's ParentBlogId if not null, else use the ID from the request
            //    isCurrent = true
            //};

            // Add the new blog entry to the database
            //_dataContext.BlogModel.Add(newBlog);
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
