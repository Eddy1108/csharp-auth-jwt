using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using workshop.webapi.DataModels;

namespace workshop.webapi.Data
{
    public class DataContext : IdentityUserContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //further information
            //https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0


            modelBuilder.Entity<Post>().HasData(new Post { Id = 1, AuthorId = "1", Content = "Clubman" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 2, AuthorId = "2", Content = "T5 California" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 3, AuthorId = "2", Content = "Up" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 4, AuthorId = "1", Content = "id5" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 5, AuthorId = "1", Content = "Golf" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 6, AuthorId = "1", Content = "Beetle" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 7, AuthorId = "1", Content = "Polo" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 8, AuthorId = "3", Content = "ForTwo" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 9, AuthorId = "3", Content = "Bournemouth" });
            modelBuilder.Entity<Post>().HasData(new Post { Id = 10, AuthorId = "2", Content = "Down" });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Post> Posts { get; set; }
    }
}
