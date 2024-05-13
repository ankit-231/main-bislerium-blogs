namespace bislerium_blogs.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        //public string BlogAuthor { get; set; }
        public DateTime UploadedTimestamp { get; set; }

        public DateTime UpdatedTimestamp { get; set; }

        public bool isCurrent { get; set; }

        // foreign key
        public string? UserId { get; set; }

        // navigation property
        public CustomUser? User { get; set; }

        // aavigation property for reactions
        public ICollection<ReactionModel>? Reactions { get; set; }

        // aavigation property for comments
        public ICollection<CommentModel>? Comments { get; set; }


        // Foreign key for the parent blog
        public int? ParentBlogId { get; set; }

        // Navigation property for the parent blog
        public BlogModel? ParentBlog { get; set; }

        // Navigation property for the child blogs
        public ICollection<BlogModel>? ChildBlogs { get; set; }

    }
}
