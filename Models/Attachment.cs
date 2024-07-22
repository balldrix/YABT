using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YetAnotherBugTracker.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Post Date")]
        [DisplayFormat(DataFormatString = "{ 0:dd/MM/yyyy HH:mm }")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string Filename { get; set; }

        [ForeignKey("Ticket")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; set; }

        [Required]
        public Ticket Ticket { get; set; }

        [ForeignKey("User")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}