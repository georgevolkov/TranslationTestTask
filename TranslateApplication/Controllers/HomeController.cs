using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TranslateApplication.Controllers
{
   public class HomeController : Controller
   {
      [Authorize]
      public IActionResult Index()
      {
         string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
         ViewBag.Role = role;
         return View();
      }

      [Authorize(Roles = "admin")]
      public IActionResult About()
      {
         return Content("Вход только для администратора");
      }

      public async Task<IActionResult> Logout()
      {
         await HttpContext.SignOutAsync();
         return RedirectToAction("Index", "Home");
      }
   }
}
