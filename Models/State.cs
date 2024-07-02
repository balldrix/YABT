using System.ComponentModel.DataAnnotations;

namespace YetAnotherBugTracker.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}