using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YetAnotherBugTracker.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("ProjectLead")]
        public string ProjectLeadId { get; set; }

        [DisplayName("Project Lead")]
        public ApplicationUser ProjectLead { get; set; }

        [DisplayName("Team Members")]
        public ICollection<ApplicationUser> Members { get; set; } = [];

        public ICollection<Ticket> Tickets { get; set; } = [];

        [ForeignKey("Author")]
		public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
