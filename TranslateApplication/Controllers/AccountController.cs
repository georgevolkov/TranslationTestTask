using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranslateAppDAL;
using TranslateAppDAL.Domain;
using TranslateApplication.Models;

namespace TranslateApplication.Controllers
{
   public class AccountController : Controller
   {
      private readonly TranslateDbContext _context;
      public AccountController(TranslateDbContext context)
      {
         _context = context;
      }
      [HttpGet]
      public IActionResult Register()
      {
         return View();
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Register(RegisterModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
               user = new User { Email = model.Email, Password = model.Password };
               var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
               if (userRole != null)
                  user.Role = userRole;

               _context.Users.Add(user);
               await _context.SaveChangesAsync();

               await Authenticate(user);

               return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
         }
         return View(model);
      }

      [HttpGet]
      public IActionResult Login()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(LoginModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
               await Authenticate(user);

               return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
         }

         return View(model);
      }
      private async Task Authenticate(User user)
      {
         var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

         var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
             ClaimsIdentity.DefaultRoleClaimType);

         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
      }
   }
}
