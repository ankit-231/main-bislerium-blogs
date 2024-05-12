namespace bislerium_blogs.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        //public string BlogAuthor { get; set; }
        public DateTime UploadedTimestamp { get; set; }

        


        // foreign key
        public string? UserId { get; set; }

        // navigation property
        public CustomUser? User { get; set; }

        // aavigation property for reactions
        public ICollection<ReactionModel>? Reactions { get; set; }

        // aavigation property for comments
        public ICollection<CommentModel>? Comments { get; set; }



    }
}
