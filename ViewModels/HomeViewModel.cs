using System.Collections.Generic;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<Project> Projects { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}