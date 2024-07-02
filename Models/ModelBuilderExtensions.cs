using Microsoft.EntityFrameworkCore;

namespace YetAnotherBugTracker.Models
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>().HasData(
                    new State { Id = 1, Name = "Backlog" },
                    new State { Id = 2, Name = "Selected for Development" },
                    new State { Id = 3, Name = "In Progress" },
                    new State { Id = 4, Name = "Ready for Testing" },
                    new State { Id = 5, Name = "Testing" },
                    new State { Id = 6, Name = "Complete" }
                );

            modelBuilder.Entity<Priority>().HasData(
                    new Priority { Id = 1, Name = "Highest" },
                    new Priority { Id = 2, Name = "Medium" },
                    new Priority { Id = 3, Name = "Low" },
                    new Priority { Id = 4, Name = "Lowest" }
                );

            modelBuilder.Entity<ItemType>().HasData(
                    new ItemType { Id = 1, Name = "Epic" },
                    new ItemType { Id = 2, Name = "Feature" },
                    new ItemType { Id = 3, Name = "User Story" },
                    new ItemType { Id = 4, Name = "Task" },
                    new ItemType { Id = 5, Name = "Bug" }
                );
        }
    }
}