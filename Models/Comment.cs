using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YetAnotherBugTracker.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Post Date")]
        [DisplayFormat(DataFormatString = "{ 0:dd/MM/yyyy HH:mm }")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

		[Required]
		public Ticket Ticket { get; set; }

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public ApplicationUser User { get; set; }

        [Required]
        public string TextComment { get; set; }
    }
}