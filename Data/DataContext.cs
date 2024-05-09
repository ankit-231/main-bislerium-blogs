using bislerium_blogs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bislerium_blogs.Data
{
    public class DataContext: IdentityDbContext<CustomUser>
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // note: configure the table names and other things related to table here
            //modelBuilder.Entity<CustomUser>();
        }
    }
}
