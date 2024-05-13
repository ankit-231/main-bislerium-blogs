using bislerium_blogs.Models;


namespace bislerium_blogs.DTO
{
    public class BlogResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<CommentModel> AllComments { get; set; }

        public int? UserReactionStatus { get; set; } = null;
        public DateTime UploadedTimestamp { get; set; }
        public DateTime? UpdatedTimestamp { get; set; }
        public string Username { get; set; }

        public UserProfile? Author { get; set; }

        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }

        public int TotalComments { get; set; }
    }
}