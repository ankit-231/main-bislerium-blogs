namespace bislerium_blogs.Services.Implementations
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> AttachmentPaths { get; set; }
    }
}
