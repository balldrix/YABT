using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Utility;

namespace YetAnotherBugTracker.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _appDbContext;
        private readonly DemoDbContext _demoDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ItemType[] itemTypes =
        {
            new ItemType { Id = 1, Name = "Epic" },
            new ItemType { Id = 2, Name = "Feature" },
            new ItemType { Id = 3, Name = "User Story" },
            new ItemType { Id = 4, Name = "Task" },
            new ItemType { Id = 5, Name = "Bug" }
        };

        private readonly Priority[] priorities =
        {
            new Priority { Id = 1, Name = "Highest" },
            new Priority { Id = 2, Name = "Medium" },
            new Priority { Id = 3, Name = "Low" },
            new Priority { Id = 4, Name = "Lowest" }
        };

        private readonly string[] roles =
        {
            DbUtility.Role_Admin,
            DbUtility.Role_Project_Manager,
            DbUtility.Role_Developer,
            DbUtility.Role_Stakeholder,
            DbUtility.Role_Demo_Admin,
            DbUtility.Role_Demo_Project_Mananger,
            DbUtility.Role_Demo_Developer,
            DbUtility.Role_Demo_Stakeholder
        };

        private readonly State[] states =
        {
            new State { Id = 1, Name = "Backlog" },
            new State { Id = 2, Name = "Selected for Development" },
            new State { Id = 3, Name = "In Progress" },
            new State { Id = 4, Name = "Ready for Testing" },
            new State { Id = 5, Name = "Testing" },
            new State { Id = 6, Name = "Complete" }
        };

        public DbInitializer(AppDbContext appDbContext,
            DemoDbContext demoDbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _demoDbContext = demoDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Init()
        {
            // add roles and default users
            AddRolesToDb();
            AddDefaultUsersToDb();

            // removes all demo data from dbs
            ClearDemoDataFromDbs();

            InitStates();
            InitPriorities();
            InitItemTypes();
            InitDemoProject();
            AddDemoTicketsToDb();
            SaveChanges();
        }

        private void ClearDemoDataFromDbs()
        {
            ClearDemoDataFromDb(_appDbContext);
            ClearDemoDataFromDb(_demoDbContext);
        }

        private void ClearDemoDataFromDb(DbContext dbContext)
        {
            RemoveDemoTickets(dbContext);
            RemoveDemoProject(dbContext);
        }

        private void InitStates()
        {
            foreach(var state in states)
            {
                if(_demoDbContext.Set<State>().FirstOrDefault(s => s.Name == state.Name) == null)
                {
                    _demoDbContext.Set<State>()
                        .Add(state);
                }
            }
        }

        private void InitPriorities()
        {
            foreach(var priority in priorities)
            {
                if(_demoDbContext.Set<Priority>().FirstOrDefault(p => p.Name == priority.Name) == null)
                {
                    _demoDbContext.Set<Priority>()
                    .Add(priority);
                }
            }
        }

        private void InitItemTypes()
        {
            foreach(var type in itemTypes)
            {
                if(_demoDbContext.Set<ItemType>().FirstOrDefault(i => i.Name == type.Name) == null)
                {
                    _demoDbContext.Set<ItemType>()
                    .Add(type);
                }
            }
        }

        private void RemoveDemoTickets(DbContext dbContext)
        {
            var demoTickets = CreateDemoTickets(dbContext);

            foreach(var ticket in demoTickets)
            {
                var ticketInDb = dbContext.Set<Ticket>().FirstOrDefault(t => t.Title == ticket.Title);
                if(ticketInDb != null)
                {
                    dbContext.Set<Ticket>().Remove(ticketInDb);
                }
            }
        }

        private void RemoveDemoProject(DbContext dbContext)
        {
            var demoProject = dbContext.Set<Project>().FirstOrDefault(p => p.Name == DbUtility.Demo_Project);
            if(demoProject != null)
            {
                dbContext.Set<Project>().Remove(demoProject);
            }
        }

        private void AddRolesToDb()
        {
            foreach(var role in roles)
            {
                if(!_roleManager.Roles.Any(r => r.Name == role))
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }
        }

        private void AddDefaultUsersToDb()
        {
            AddUser("ecGqT6vX4G", "admin@gmail.com", "Christopher Ball", "XnM6Gx6zMh", DbUtility.Role_Admin);
            AddUser("Demo_Admin", "demo_admin@gmail.com", "Demo Admin", "DemoUserPassword", DbUtility.Role_Demo_Admin);
            AddUser("Demo_ProjectManager", "demo_project_manager@gmail.com", "Demo Project Manager", "DemoUserPassword", DbUtility.Role_Demo_Project_Mananger);
            AddUser("Demo_Developer", "demo_developer@gmail.com", "Demo Developer", "DemoUserPassword", DbUtility.Role_Demo_Developer);
            AddUser("Demo_Stakeholder", "demo_stakeholder@gmail.com", "Demo StakeHolder", "DemoUserPassword", DbUtility.Role_Demo_Stakeholder);
        }

        private void AddUser(string userName,
                            string email,
                            string name,
                            string password,
                            string role)
        {
            if(_userManager.Users.Where(u => u.Email == email).FirstOrDefault() == null)
            {
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true,
                    Role = role

                }, password).GetAwaiter().GetResult();

                ApplicationUser user = _userManager.Users.Where(u => u.Email == email).FirstOrDefault();

                _userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
            }
        }

        private void SaveChanges()
        {
            _appDbContext.SaveChangesAsync();
            _demoDbContext.SaveChangesAsync();
        }

        private void InitDemoProject()
        {
            AddProjectToDb(_appDbContext);
            AddProjectToDb(_demoDbContext);

            AddDefaultMembersToDemoProject(_appDbContext);
            AddDefaultMembersToDemoProject(_demoDbContext);
        }

        private void AddProjectToDb(DbContext dbContext)
        {
            var demoProject = new Project()
            {
                Name = DbUtility.Demo_Project,
                Author = _userManager.Users
                    .Where(u => u.Name == "Demo Admin").FirstOrDefault(),
                ProjectLead = _userManager.Users
                    .Where(u => u.Name == "Demo Project Manager").FirstOrDefault(),
            };

            if(dbContext.Set<Project>()
                .Where(p => p.Name == DbUtility.Demo_Project)
                .FirstOrDefault() == null)
            {
                dbContext.Set<Project>().Add(demoProject);
                dbContext.SaveChangesAsync().GetAwaiter().GetResult();
            }
        }

        private void AddDefaultMembersToDemoProject(DbContext dbContext)
        {
            var demoProject = dbContext.Set<Project>()
                .Where(p => p.Name == DbUtility.Demo_Project)
                .FirstOrDefault();

            demoProject.Members = new List<ApplicationUser>();

            var _demoMembers = new List<ApplicationUser>()
            {
                _userManager.Users
                    .FirstOrDefault(u => u.Name == DbUtility.Role_Demo_Project_Mananger),
                _userManager.Users
                    .FirstOrDefault(u => u.Name == DbUtility.Role_Demo_Developer),
                _userManager.Users
                    .FirstOrDefault(u => u.Name == DbUtility.Role_Demo_Stakeholder),
            };

            foreach(var member in _demoMembers)
            {
                demoProject.Members.Add(member);
            }

            dbContext.Set<Project>().Update(demoProject);
            dbContext.SaveChangesAsync().GetAwaiter().GetResult();
        }

        private void AddDemoTicketsToDb()
        {
            AddDemoTicketsToDb(_appDbContext);
            AddDemoTicketsToDb(_demoDbContext);
        }

        private void AddDemoTicketsToDb(DbContext dbContext)
        {
            List<Ticket> tickets = CreateDemoTickets(dbContext);

            foreach(var ticket in tickets)
            {
                if(dbContext.Set<Ticket>()
                    .Where(t => t.Title == ticket.Title)
                    .FirstOrDefault() == null)
                {
                    dbContext.Set<Ticket>().Add(ticket);
                    dbContext.Set<Project>().Update(ticket.Project);
                }
            }
        }

        private List<Ticket> CreateDemoTickets(DbContext dbContext)
        {
            return new List<Ticket>()
            {
                new Ticket()
                {
                    AssignedUser = _userManager.Users.FirstOrDefault(u => u.Name == "Demo Developer"),
                    Author = _userManager.Users.FirstOrDefault(u => u.Name == "Demo Project Manager"),
                    Description = "An example ticket with Developer as assigned user.",
                    Title = "Demo User Story",
                    Type = dbContext.Set<ItemType>().FirstOrDefault(t => t.Name == "User Story"),
                    Priority = dbContext.Set<Priority>().FirstOrDefault(p => p.Name == "Medium"),
                    State = dbContext.Set<State>().FirstOrDefault(s => s.Name == "Selected for Development"),
                    Project = dbContext.Set<Project>().FirstOrDefault(p => p.Name == DbUtility.Demo_Project)
                },
                new Ticket()
                {
                    Author = _userManager.Users.FirstOrDefault(u => u.Name == "Demo Stakeholder"),
                    Description = "An example ticket where a stakeholder has found a Bug",
                    Title = "Demo Bug",
                    Type = dbContext.Set<ItemType>().FirstOrDefault(t => t.Name == "Bug"),
                    Priority = dbContext.Set<Priority>().FirstOrDefault(p => p.Name == "Highest"),
                    State = dbContext.Set<State>().FirstOrDefault(s => s.Name == "Backlog"),
                    Project = dbContext.Set<Project>().FirstOrDefault(p => p.Name == DbUtility.Demo_Project)
                }
            };
        }
    }
}
