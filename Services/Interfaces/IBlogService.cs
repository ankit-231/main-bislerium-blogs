

using bislerium_blogs.DTO;
using Microsoft.AspNetCore.Mvc;

namespace bislerium_blogs.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IActionResult> GetAllBlogs(string? userId = null, string? _sortBy = null, PaginationFilter? filter = null);
        Task<IActionResult> GetHistory(int id, bool isBlog = true);
        Task<IActionResult> GetPaginatedBlogs(string? userId = null, string? _sortBy = null, PaginationFilter? filter = null, int? total = null, string? timeType = null);
        Task<IActionResult> GetTopTenAuthors(string? timeType = null);
    }
}
