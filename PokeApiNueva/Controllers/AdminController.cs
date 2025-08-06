using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeApiNueva.Services;

namespace PokeApiNueva.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            var users = _adminService.UsersList;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> SwitchRole(int userId, string newRole)
        {
            try
            {
                var success = await _adminService.SwitchRoleAsync(userId, newRole);
                if (!success)
                {
                    TempData["Error"] = "The user was not found or the role is invalid.";
                }
                else
                {
                    TempData["Success"] = "Role updated successfully.";
                }
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
