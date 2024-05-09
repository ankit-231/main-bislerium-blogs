namespace bislerium_blogs.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }

        // foreign key
        public string UserId { get; set; } 
        public int BlogId { get; set; } 


        // Navigation property for reactions
        public ICollection<ReactionModel> Reactions { get; set; }
        public CustomUser User { get; set; }
        public BlogModel Blog { get; set; }

        // Reference to the parent comment
        public int? ParentCommentId { get; set; }
        public CommentModel ParentComment { get; set; }
    }
}
