using System.Collections.Generic;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}