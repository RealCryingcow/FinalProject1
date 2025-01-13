using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.Repositories.RepositoryInterfaces;
using QuestionnarieApp_.ViewModels;

namespace QuestionnarieApp_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        public AdminController(UserManager<ApplicationUser> userManager, IMapper mapper, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _messageRepository = messageRepository;
        }

        // GET: /Admin/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Admin/ManageUsers
        public IActionResult ManageUsers()
        {
            var users = _userManager.Users.ToList();
            var viewModels = _mapper.Map<List<ManageUserViewModel>>(users);
            return View(viewModels);
        }

        public async Task<IActionResult> EditRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var allRoles = new[] { "Admin", "User" }; // List of all possible roles

            var model = new EditRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                SelectedRoles = currentRoles.ToList()
            };

            ViewBag.AllRoles = allRoles;
            return View(model);
        }

        // POST: /Admin/EditRoles/{userId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove roles not in SelectedRoles
            var rolesToRemove = currentRoles.Except(model.SelectedRoles).ToList();
            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            // Add roles in SelectedRoles that are not already assigned
            var rolesToAdd = model.SelectedRoles.Except(currentRoles).ToList();
            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> Messages()
        {
            var users = await _messageRepository.GetAllUsersWithMessagesAsync();

            var userDetails = await _userManager.Users
                .Where(u => users.Contains(u.Id))
                .ToListAsync();

            return View(userDetails);
        }

        public async Task<IActionResult> Conversation(string userId)
        {
            var messages = await _messageRepository.GetMessagesByUserIdAsync(userId);
            ViewData["UserId"] = userId;

            return View(messages);
        }


    }

}
