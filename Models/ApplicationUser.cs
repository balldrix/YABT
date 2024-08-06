using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace YetAnotherBugTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public ICollection<Project> Projects { get; set; } = [];
    }
}
