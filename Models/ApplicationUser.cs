using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace YetAnotherBugTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
