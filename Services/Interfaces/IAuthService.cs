using bislerium_blogs.DTO;

namespace bislerium_blogs.Services.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterRequestPayload registerPayload);

    }
}
