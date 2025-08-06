using E_Commerce.Context;
using E_Commerce.Helper;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    //[Authorize]
    public class AccountController(E_CommerceContext context) : Controller
    {
        private readonly E_CommerceContext _context = context;

        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.RoleList = Enum.GetValues(typeof(Role)).Cast<Role>().Where(r => r != Role.Admin).ToList();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoleList = Enum.GetValues(typeof(Role)).Cast<Role>().Where(r => r != Role.Admin).ToList();
                return View(model);
            }

            var existUser = await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email && !u.Is_Deleted);
            if (existUser)
            {
                ModelState.AddModelError("", "Username or email already exists.");
                return View(existUser);
            }

            var passwordHash = PasswordHelper.Hash(model.Password);
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Role = model.Role,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registered successfully!";
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Already logged in → Go to Product list
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var allUser = await _context.Users.Where(u => !u.Is_Deleted).ToListAsync();

            var hashed = PasswordHelper.Hash(model.Password);
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                (u.Username == model.Login|| u.Email == model.Login) &&
                u.PasswordHash == hashed && !u.Is_Deleted);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Email),
                    new("UserId", user.Id.ToString()),
                    new(ClaimTypes.Role, user.Role.ToString())
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError("", "Invalid login.");
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var email = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == email && !u.Is_Deleted);

            if (user == null) return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(UserViewModel model)
        {
            var email = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == email && !u.Is_Deleted);

            if (user == null) return RedirectToAction("Login");

            if (!ModelState.IsValid) return View(model);

            user.Email = model.Email;
            user.Username = model.Username;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            _context.SaveChanges();
            ViewBag.Message = "Profile updated successfully!";
            return RedirectToAction("Index", "Home");

            //return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
