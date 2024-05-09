using bislerium_blogs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bislerium_blogs.Data
{
    public class DataContext : IdentityDbContext<CustomUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        // note new table then this is how you add it
        public DbSet<BlogModel> BlogModel { get; set; }

        public DbSet<ReactionModel> ReactionModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogModel>()
               .HasOne(b => b.User)
               .WithMany(u => u.Blogs)
               .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<BlogModel>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<CommentModel>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CommentModel>()
                .HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId);


            modelBuilder.Entity<ReactionModel>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<ReactionModel>()
                .HasOne(r => r.Blog)
                .WithMany(b => b.Reactions)
                .HasForeignKey(r => r.BlogId);

            modelBuilder.Entity<ReactionModel>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => r.CommentId);



            // note: configure the table names and other things related to table here
            //modelBuilder.Entity<CustomUser>();
        }
    }
}
