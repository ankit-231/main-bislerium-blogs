using bislerium_blogs.Data;
using bislerium_blogs.DTO;
using bislerium_blogs.Models;
using bislerium_blogs.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;


namespace bislerium_blogs.Services.Implementations
{
    public class BlogService : IBlogService
    {
        //private readonly SignInManager<CustomUser> _signInManager;
        //private readonly UserManager<CustomUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        //private readonly IEmailService _emailService;

        public BlogService(DataContext context/*, SignInManager<CustomUser> signInManager, UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService*/)
        {
            //_signInManager = signInManager;
            //_userManager = userManager;
            //_roleManager = roleManager;
            //_emailService = emailService;
            _context = context;
        }

        public async Task<IActionResult> GetAllBlogs(string? userId = null, string? _sortBy = null, PaginationFilter? filter = null)
        {
            var blogs = new List<BlogModel>();
            Console.WriteLine("hiiiii in service");
            Console.WriteLine(_sortBy);
            Console.WriteLine(filter);
            Console.WriteLine(filter.PageNumber);
            Console.WriteLine(filter.PageSize);

            IQueryable<BlogModel> query = _context.BlogModel.Include(blog => blog.User);

            //if (filter != null)
            //{
            //    query.Skip((filter.PageNumber - 1) * filter.PageSize)
            //         .Take(filter.PageSize);
            //}

            if (userId != null)
            {
                query = query.Where(blog => blog.UserId == userId);
            }

            //random sorting
            if (_sortBy == "random")
            {
                var random = new Random();
                // Execute query and retrieve all blogs


                if (filter != null)
                {
                    query.Skip((filter.PageNumber - 1) * filter.PageSize)
                         .Take(filter.PageSize);
                }

                blogs = await query.ToListAsync();

                //ef does not support random next() in sql
                var shuffledBlogs = blogs.OrderBy(blog => random.Next()).ToList();
                blogs = shuffledBlogs;
            }
            else
            {
                if (_sortBy == "recent")
                {
                    query = query.OrderByDescending(blog => blog.UploadedTimestamp);
                }
                else if (_sortBy == "popular")
                {
                    query = query.OrderBy(blog => blog.UploadedTimestamp);
                }
                if (filter != null)
                {
                    Console.WriteLine("filte is not null");
                    query.Skip((filter.PageNumber - 1) * filter.PageSize)
                         .Take(filter.PageSize);
                }
                blogs = await query.ToListAsync();

            }

            // Apply pagination after shuffling or sorting
            if (filter != null)
            {
                blogs = blogs.Skip((filter.PageNumber - 1) * filter.PageSize)
                             .Take(filter.PageSize)
                             .ToList();
            }


            int? userReactionStatus = null;

            Console.WriteLine(blogs.Count);

            var blogDtos = new List<BlogResponseDTO>();
            foreach (var blog in blogs)
            {

                if (userId != null)
                {
                    userReactionStatus = await GetReactionStatusForUser(blog.Id, userId);
                }

                //var totalLikes = await _context.ReactionModel.CountAsync(l => l.BlogId == blog.Id);
                var totalLikes = await BlogReactionCount(blog.Id, "like");
                var totalDisLikes = await BlogReactionCount(blog.Id, "dislike");
                var totalComments = await TotalComments(blog.Id);
                var allComments = await GetAllComments(blog.Id);
                //var author = blog.User;
                System.Diagnostics.Debug.WriteLine(blog.User);
                System.Diagnostics.Debug.WriteLine(blog.UserId);
                System.Diagnostics.Debug.WriteLine("whyyyyyyyy");
                var blogDto = new BlogResponseDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    TotalLikes = totalLikes,
                    TotalDislikes = totalDisLikes,
                    TotalComments = totalComments,
                    UserReactionStatus = userReactionStatus,
                    UploadedTimestamp = blog.UploadedTimestamp,
                    UpdatedTimestamp = blog.UpdatedTimestamp,
                    //AllComments = allComments,
                    //CreatedAt = blog.CreatedAt,
                    //UpdatedAt = blog.UpdatedAt,
                    //IsCurrent = blog.IsCurrent,
                    Author = blog.User != null ? new UserProfile
                    {
                        UserId = blog.User.Id,
                        Email = blog.User.Email,
                        FirstName = blog.User.FirstName,
                        LastName = blog.User.LastName,
                    } : null

                    //TotalLikes = blog.Likes.Count
                };

                blogDtos.Add(blogDto);
            }

            return new OkObjectResult(blogDtos);
        }



        public async Task<int> BlogReactionCount(int blogId, string ReactionType)
        {
            var reactionStatus = 1;
            if (ReactionType == "like")
            {
                reactionStatus = 1;
            }
            else
            {
                reactionStatus = 0;
            }

            var totalLikes = await _context.ReactionModel
                    .CountAsync(r => r.BlogId == blogId && r.ReactionStatus == reactionStatus && r.CommentId == null);
            return totalLikes;
        }

        public async Task<int> TotalComments(int blogId)
        {
            var totalComments = await _context.CommentModel
                .CountAsync(c => c.BlogId == blogId && c.ParentCommentId == null);

            return totalComments;
        }

        public async Task<List<CommentDTO>> GetAllComments(int blogId/*, int commentID*/)
        {
            var comments = await _context.CommentModel
                .Where(c => c.BlogId == blogId || c.ParentCommentId == null)
                .ToListAsync();

            var commentDtos = new List<CommentDTO>();

            foreach (var comment in comments)
            {
                var commentDto = new CommentDTO
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    UserId = comment.UserId,
                    BlogId = comment.BlogId,
                    ParentCommentId = comment.ParentCommentId
                };

                commentDtos.Add(commentDto);
            }

            return commentDtos;
        }

        public async Task<int?> GetReactionStatusForUser(int blogId, string userId, int? commentId = null)
        {
            var reaction = await _context.ReactionModel
                .FirstOrDefaultAsync(r => r.BlogId == blogId && r.UserId == userId && r.CommentId == commentId);

            if (reaction != null)
            {
                return reaction.ReactionStatus;
            }

            return null; // Return null if no reaction is found
        }

        public async Task<IActionResult> GetHistory(int id, bool isBlog = true)
        {
            //return await _context.HistoryLog
            //    .Where(log => log.EntityId == id && log.IsBlog)
            //    .ToListAsync();

            var history = await _context.HistoryLog
                .Where(log => log.EntityId == id && log.IsBlog)
                .ToListAsync();

            return new OkObjectResult(history);

        }

    }
}
