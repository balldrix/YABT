using System.ComponentModel.DataAnnotations;

namespace YetAnotherBugTracker.Models
{
    public class ItemType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}