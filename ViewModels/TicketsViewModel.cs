using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;

namespace YetAnotherBugTracker.ViewModels
{
    public class TicketsViewModel
    {
        public IFormFile Attachment { get; set; }
        public int AttachmentId { get; set; }
        public string AssignedUserId { get; set; }
        public string TextComment { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public int ProjectId { get; set; }
        public RolePermissions RolePermissions { get; set; }
        public string SearchTerm { get; set; }
        public int StateId { get; set; }
        public Ticket Ticket { get; set; }
        public int? TicketId { get; set; }

        [Required]
        public string Title { get; set; }
        public int TypeId { get; set; }

        public IEnumerable<ItemType> ItemTypes { get; set; }
        public IEnumerable<Priority> Priorities { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<State> StateList { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
        public List<SelectListItem> UserOptions { get; set; }
    }
}
