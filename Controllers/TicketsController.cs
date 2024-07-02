using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;
using YetAnotherBugTracker.Utility;
using YetAnotherBugTracker.ViewModels;

namespace YetAnotherBugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IRepository<ItemType> _itemTypeRepository;
        private readonly IRepository<Priority> _priorityRepository;
        private readonly IRoleFactory _roleFactory;
        private readonly IRepository<State> _stateRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(IRepository<Attachment> attachmentRepository,
                                 IWebHostEnvironment hostingEnvironment,
                                 IRepository<ItemType> itemTypeRepository,
                                 IRepository<Priority> priorityRepository,
                                 IRoleFactory roleFactory,
                                 IRepository<State> stateRepository,
                                 UserManager<ApplicationUser> userManager)
        {
            _attachmentRepository = attachmentRepository;
            _hostingEnvironment = hostingEnvironment;
            _itemTypeRepository = itemTypeRepository;
            _priorityRepository = priorityRepository;
            _roleFactory = roleFactory;
            _stateRepository = stateRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAttachment(TicketsViewModel ticketsViewModel)
        {
            if(ticketsViewModel.Attachment != null)
            {
                string folder = Path.Combine(_hostingEnvironment.WebRootPath, "attachments");
                string uniqueName = Guid.NewGuid().ToString() + "_" + ticketsViewModel.Attachment.FileName;
                string filePath = Path.Combine(folder, uniqueName);

                FileStream filestream = new FileStream(filePath, FileMode.Create);
                ticketsViewModel.Attachment.CopyTo(filestream);
                filestream.Close();

                var applicationUser = await _userManager.GetUserAsync(User);
                var roleObject = _roleFactory.GetRole(applicationUser);

                var ticket = roleObject.GetTicketsForUserRole(applicationUser)
                    .First(t => t.Id == ticketsViewModel.TicketId);

                var attachment = new Attachment
                {
                    Date = DateAndTime.Now,
                    Filename = ticketsViewModel.Attachment.FileName,
                    FilePath = uniqueName,
                    Ticket = ticket,
                    User = await _userManager.FindByNameAsync(User.Identity.Name)
                };

                ticket.Attachments.Add(attachment);
                roleObject.UpdateTicket(ticket);

                return RedirectToAction("Details", new { id = ticketsViewModel.TicketId });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(TicketsViewModel viewModel)
        {
            if(viewModel.TicketId != null)
            {
                var applicationUser = await _userManager.GetUserAsync(User);
                var roleObject = _roleFactory.GetRole(applicationUser);

                var ticket = roleObject.GetTicketsForUserRole(applicationUser)
                    .First(t => t.Id == viewModel.TicketId);

                viewModel.Ticket = ticket;

                var comment = new Comment
                {
                    Date = DateAndTime.Now,
                    TextComment = viewModel.TextComment,
                    User = await _userManager.FindByNameAsync(User.Identity.Name)
                };

                ticket.Comments.Add(comment);
                roleObject.UpdateTicket(ticket);

                return RedirectToAction("Details", new { id = viewModel.Ticket.Id });
            }

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var viewModel = new TicketsViewModel()
            {
                Priorities = _priorityRepository.AllItems,
                ItemTypes = _itemTypeRepository.AllItems,
                StateList = _stateRepository.AllItems
            };

            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            viewModel.Projects = roleObject.GetProjectsForUserRole(applicationUser);

            if(id == null)
            {
                viewModel.Tickets = roleObject.GetTicketsForUserRole(applicationUser).OrderBy(t => t.PriorityID);
            }
            else
            {
                viewModel.Tickets = roleObject.GetTicketsForUserRole(applicationUser)
                .OrderBy(t => t.PriorityID)
                .Where(t => t.ProjectID == id);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            var projects = roleObject.GetProjectsForUserRole(applicationUser);

            var rolePermissions = new RolePermissions()
            {
                CanAssignUser = roleObject.Permissions.CanAssignUser
            };

            var viewModel = new TicketsViewModel
            {
                RolePermissions = rolePermissions,
                ItemTypes = _itemTypeRepository.AllItems,
                Priorities = _priorityRepository.AllItems,
                StateList = _stateRepository.AllItems,
                ProjectId = roleObject.GetProjectsForUserRole(applicationUser).Last().Id,
                PriorityId = _priorityRepository.AllItems
                        .FirstOrDefault(p => p.Name == "Medium").Id,
                StateId = _stateRepository.AllItems
                        .FirstOrDefault(s => s.Name == "Backlog").Id,
                TypeId = _itemTypeRepository.AllItems
                        .FirstOrDefault(t => t.Name == "Feature").Id,
                Projects = projects,
                Tickets = roleObject.GetTicketsForUserRole(applicationUser),
            };

            if(viewModel.Projects.Count() == 0)
            {
                return View("NoProjectsAssigned");
            }

            viewModel.UserOptions = await DeveloperOptions();

            return View(viewModel);
        }

        private async Task<List<SelectListItem>> DeveloperOptions()
        {
            var developers = await _userManager.GetUsersInRoleAsync(DbUtility.Role_Developer);

            var demoDeveloper = _userManager.Users.First(u => u.UserName == "Demo_Developer");

            developers.Add(demoDeveloper);

            var userOptions = developers
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.Name
                    })
                    .ToList();

            var emptyOption = new SelectListItem
            {
                Value = "",
                Text = "None",
                Selected = true
            };

            userOptions.Add(emptyOption);

            return userOptions;
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(TicketsViewModel ticketsViewModel)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            if(ticketsViewModel.TicketId == null)
            {
                if(ModelState.IsValid)
                {
                    var newTicket = new Ticket
                    {
                        Title = ticketsViewModel.Title,
                        Description = ticketsViewModel.Description,

                        Priority = _priorityRepository.AllItems.First(p => p.Id == ticketsViewModel.PriorityId),
                        State = _stateRepository.AllItems.First(s => s.Id == ticketsViewModel.StateId),
                        Type = _itemTypeRepository.AllItems.First(t => t.Id == ticketsViewModel.TypeId),
                        Project = roleObject.GetProjectsForUserRole(applicationUser).First(p => p.Id == ticketsViewModel.ProjectId),
                        Author = _userManager.Users.First(u => u.UserName == User.Identity.Name)
                    };

                    if(ticketsViewModel.AssignedUserId != null)
                    {
                        newTicket.AssignedUser = _userManager.Users.First(u => u.Id == ticketsViewModel.AssignedUserId);
                    }
                    else
                    {
                        newTicket.AssignedUser = null;
                    }

                    roleObject.AddNewTicket(newTicket);

                    return RedirectToAction("Index", new { newTicket.Project.Id });
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var ticket = roleObject.GetTicketsForUserRole(applicationUser)
                        .First(t => t.Id == ticketsViewModel.TicketId);

                    ticket.Title = ticketsViewModel.Title;
                    ticket.Description = ticketsViewModel.Description;
                    ticket.Priority = _priorityRepository.Get(ticketsViewModel.PriorityId);
                    ticket.State = _stateRepository.Get(ticketsViewModel.StateId);
                    ticket.Type = _itemTypeRepository.Get(ticketsViewModel.TypeId);
                    ticket.Project = roleObject.GetProjectsForUserRole(applicationUser)
                        .First(p => p.Id == ticket.ProjectID);

                    if(ticketsViewModel.AssignedUserId != null)
                    {
                        ticket.AssignedUser = _userManager.Users.First(u => u.Id == ticketsViewModel.AssignedUserId);
                    }
                    else
                    {
                        ticket.AssignedUser = null;
                    }

                    roleObject.UpdateTicket(ticket);

                    return RedirectToAction("Details", new { id = ticket.Id });
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if(searchTerm == null)
            {
                return RedirectToAction("Index");
            }

            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            var viewModel = new TicketsViewModel
            {
                Tickets = roleObject.SearchUserTickets(applicationUser, searchTerm).OrderBy(t => t.PriorityID)
            };

            return View("Index", viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);
            var tickets = roleObject.GetTicketsForUserRole(applicationUser).OrderBy(t => t.PriorityID);

            var viewModel = new TicketsViewModel
            {
                Ticket = tickets.FirstOrDefault(t => t.Id == id),
                RolePermissions = roleObject.Permissions
            };
            
            if(viewModel.Ticket == null)
            {
                return NotFound();
            }
            else
            {
                return View(viewModel);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAttachment(TicketsViewModel ticketsViewModel)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);
            var tickets = roleObject.GetTicketsForUserRole(applicationUser).OrderBy(t => t.PriorityID);
            var ticket = tickets.First(t => t.Id == ticketsViewModel.TicketId);

            if(ticket != null)
            {
                var attachment = _attachmentRepository.Get(ticketsViewModel.AttachmentId);

                if(attachment != null)
                {
                    ticket.Attachments.Remove(attachment);
                    roleObject.UpdateTicket(ticket);

                    string folder = Path.Combine(_hostingEnvironment.WebRootPath, "attachments");
                    string filePath = Path.Combine(folder, attachment.FilePath);

                    if(System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                return RedirectToAction("Details", new { id = ticket.Id });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            var projects = roleObject.GetProjectsForUserRole(applicationUser);
            var tickets = roleObject.GetTicketsForUserRole(applicationUser).OrderBy(t => t.PriorityID);

            var ticket = tickets.First(p => p.Id == id);

            if(ticket == null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = new TicketsViewModel
                {
                    RolePermissions = roleObject.Permissions,
                    Ticket = ticket,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    TicketId = ticket.Id,
                    ItemTypes = _itemTypeRepository.AllItems,
                    Priorities = _priorityRepository.AllItems,
                    Projects = projects,
                    StateList = _stateRepository.AllItems,
                    Tickets = tickets,
                    ProjectId = ticket.ProjectID,
                    PriorityId = ticket.PriorityID,
                    StateId = ticket.StateID,
                    TypeId = ticket.TypeID,
                };

                viewModel.UserOptions = await DeveloperOptions();

                foreach(var option in viewModel.UserOptions)
                {
                    if(option.Value == ticket.AssignedUserID)
                    {
                        option.Selected = true;
                    }
                }

                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);
            var tickets = roleObject.GetTicketsForUserRole(applicationUser);
            var projectID = tickets.First(t => t.Id == id);

            roleObject.DeleteUserTicket(applicationUser, id);

            return RedirectToAction("Index", new { id = projectID });
        }
    }
}