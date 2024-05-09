using bislerium_blogs.Services.Implementations;
//using bislerium_blogs.Services.Interfaces;

namespace bislerium_blogs.Services.Interfaces
{
    public interface IGmailEmailProvider
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
