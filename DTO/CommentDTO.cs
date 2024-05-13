using bislerium_blogs.Models;


namespace bislerium_blogs.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? UserId { get; set; }
        public int? BlogId { get; set; }
        public int? ParentCommentId { get; set; }









    }
}