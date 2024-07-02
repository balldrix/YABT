using System;
using System.Linq.Expressions;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.Utility
{
    public static class Predicates
    {
        public static Expression<Func<Ticket, bool>> ProjectManagerTickets(ApplicationUser user)
        {
            return t =>
                    (t.Project.ProjectLead != null && t.Project.ProjectLead == user) ||
                    (t.Project.Author != null && t.Project.Author == user) ||
                    (t.Author != null && t.Author == user) ||
                    (t.AssignedUser != null && t.AssignedUser == user) ||
                    t.Project.Members.Contains(user);
        }

        public static Expression<Func<Ticket, bool>> DeveloperTickets(ApplicationUser user)
        {
            return t => t.Project.Members.Contains(user) ||
                   (t.Author != null && t.Author == user) ||
                   (t.AssignedUser != null && t.AssignedUser == user);
        }

        public static Expression<Func<Project, bool>> ProjectManagerProjects(ApplicationUser user)
        {
            return p => (p.ProjectLead != null && p.ProjectLead == user) ||
                                p.Members.Contains(user) ||
                                (p.Author != null && p.Author == user);
        }
    }
}