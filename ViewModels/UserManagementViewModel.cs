using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.ViewModels
{
    public class UserManagementViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [Compare("Password", ErrorMessage = "Password entered does not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public bool IsEditRoleHidden { get; set; }
    }
}
