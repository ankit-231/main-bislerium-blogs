namespace bislerium_blogs.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendForgotPasswordEmailAsync(string firstName, string lastName, string toEmail, string passwordResetToken);

        Task SendEmailConfirmationEmailAsync(string firstName, string lastName, string userId, string email, string token);
    }
}