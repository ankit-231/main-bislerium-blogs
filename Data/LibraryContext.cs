using bislerium_blogs.Models;
using Microsoft.EntityFrameworkCore;
//using MySql.EntityFrameworkCore.Extensions;

namespace bislerium_blogs.Data
{
    //public class LibraryContext : DbContext
    //{
    //    public DbSet<Book> Book { get; set; }

    //    public DbSet<Publisher> Publisher { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseMySQL("server=localhost;database=library;user=user;password=password");
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        modelBuilder.Entity<Publisher>(entity =>
    //        {
    //            entity.HasKey(e => e.ID);
    //            entity.Property(e => e.Name).IsRequired();
    //        });

    //        modelBuilder.Entity<Book>(entity =>
    //        {
    //            entity.HasKey(e => e.ISBN);
    //            entity.Property(e => e.Title).IsRequired();
    //            entity.HasOne(d => d.Publisher)
    //              .WithMany(p => p.Books);
    //        });
    //    }
    //}
}
