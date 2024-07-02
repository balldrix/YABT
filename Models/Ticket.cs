using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YetAnotherBugTracker.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TypeID")]
        public int TypeID { get; set; }

        [Required]
        public ItemType Type { get; set; }

        [Required]
        public string Title { get; set; }

        [ForeignKey("StateID")]
        public int StateID { get; set; }

        [Required]
        public State State { get; set; }

        [ForeignKey("PriorityID")]
        public int PriorityID { get; set; }

        [Required]
        public Priority Priority { get; set; }
        
        [ForeignKey("ProjectID")]
        public int ProjectID { get; set; }

        [Required]
        public Project Project { get; set; }

        public string Description { get; set; }

        [ForeignKey("AssignedUserID")]
        public string AssignedUserID { get; set; }
        
        public ApplicationUser AssignedUser { get; set; }

        [DisplayName("Comments")]
        public ICollection<Comment> Comments { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        [DisplayName("Author")]
        public ApplicationUser Author { get; set; }

        [DisplayName("Attachments")]
        public ICollection<Attachment> Attachments { get; set; }
    }
}
