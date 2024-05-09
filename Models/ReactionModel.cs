namespace bislerium_blogs.Models
{
    public class ReactionModel
    {
        public int Id { get; set; }
        public int? ReactionStatus { get; set; } // 1, 0, or null

        // Foreign key properties
        public string UserId { get; set; }
        public int? BlogId { get; set; }
        public int? CommentId { get; set; }

        // Navigation properties
        public CustomUser? User { get; set; }
        public BlogModel? Blog { get; set; }
        public CommentModel? Comment { get; set; }
    }
}
