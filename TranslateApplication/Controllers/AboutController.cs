using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TranslateApplication.Controllers
{
   public class AboutController : Controller
   {
      [Authorize(Roles = "admin")]
      public IActionResult Index()
      {
         var model = HttpContext.User.Identity;
         return View(model);
      }
   }
}
