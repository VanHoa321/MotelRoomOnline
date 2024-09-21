using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return Redirect("/Login/Index");
            }
            return View();
        }

        public IActionResult Logout()
        {
            Functions.account = null;
            Functions.message = string.Empty;
            return RedirectToAction("Index", "Home");
        }  
    }
}
