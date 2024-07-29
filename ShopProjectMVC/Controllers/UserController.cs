using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login() { 
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user) {
            var userDb = await _userService.Login(user.Email, user.Password);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user) {
            user.Role = Role.Client;
            user.CreatedAt = DateTime.UtcNow;
            await _userService.Register(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
