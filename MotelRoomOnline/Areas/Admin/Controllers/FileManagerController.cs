using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileManagerController : Controller
    {
        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            return View();
        }
    }
}
