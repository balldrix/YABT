using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.ViewModels
{
    public class ProjectsViewModel
    {
        [Required]
        [Display(Name = "Project Name")]
        public Project Project { get; set; }
        public string ProjectLeadId { get; set; }
        public ICollection<Project> Projects { get; set; }
        public List<SelectListItem> ProjectLeadOptions { get; set; }
        public MultiSelectList MemberOptions { get; set; }
        public string SearchTerm { get; set; }
        public string[] SelectedMembers { get; set; }
        public string MemberId { get; set; }
        public bool CanAmendMembers { get; set; }
        public bool CanAddProjects { get; set; }
    }
}
