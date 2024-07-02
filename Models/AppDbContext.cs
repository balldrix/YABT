using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace YetAnotherBugTracker.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Attachment> Attachment { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<ItemType> ItemType { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SeedData();
        }
    }
}
