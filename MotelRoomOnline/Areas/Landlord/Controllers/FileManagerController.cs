using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class FileManagerController : Controller
    {
        public IActionResult Index()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            return View();
        }
    }
}
