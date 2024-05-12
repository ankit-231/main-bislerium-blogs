namespace bislerium_blogs.Models
{
    public class HistoryLog
    {
        public int Id { get; set; }
        
        public string OldContent { get; set; }

        public string NewContent { get; set; }

        public string UserId { get; set; }

        public DateTime Timestamp { get; set; }

        // Indicates whether the history entry is for a blog or a comment
        public bool IsBlog { get; set; }

        // ID of the related blog or comment
        public int EntityId { get; set; }

        public CustomUser? User { get; set; }



    }
}
