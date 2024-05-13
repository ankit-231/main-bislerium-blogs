

using Microsoft.AspNetCore.Mvc;

namespace bislerium_blogs.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IActionResult> GetAllBlogs(string? userId = null);
        Task<IActionResult> GetHistory(int id, bool isBlog = true);
    }
}
