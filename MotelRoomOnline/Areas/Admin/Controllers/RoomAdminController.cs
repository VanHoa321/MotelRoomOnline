using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomAdminController : Controller
    {
        private readonly DataContext _context;

        public RoomAdminController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            ViewBag.roomStatus = _context.RoomStatuses.ToList();
            var items = _context.Rooms.Where(r => r.RoomStatusId == 5).OrderByDescending(r => r.RoomId).ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult Approve(int? id)
        {
            var item = _context.Rooms.Find(id);
            if (item != null)
            {
                item.RoomStatusId = 1;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
