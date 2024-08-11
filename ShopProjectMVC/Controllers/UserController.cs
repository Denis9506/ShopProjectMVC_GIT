using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;
using ShopProjectMVC.Filters;
using System;
using System.Threading.Tasks;

namespace ShopProjectMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] User user)
        {
            try
            {
                ModelState.Remove(nameof(user.Orders));

                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                var userDb = await _userService.Login(user.Email, user.Password);
                if (userDb == null)
                {
                    ViewBag.ErrorMessage = "Account not found.";
                    return View(user);
                }

                HttpContext.Session.SetString("user", userDb.Name);
                HttpContext.Session.SetInt32("role", (int)userDb.Role);
                HttpContext.Session.SetInt32("userId", (int)userDb.Id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                ModelState.Remove(nameof(user.Orders));

                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                user.Role = Role.Client;
                user.CreatedAt = DateTime.UtcNow;
                await _userService.Register(user);
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(user);
            }
        }

        [AuthorizeFilter]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToAction("Login", "User");
            }
        }
    }
}
