using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostCommentController : Controller
    {
        private readonly DataContext _context;

        public PostCommentController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Posts.OrderByDescending(p => p.PostId).ToList();
            ViewBag.List = _context.PostCategories.ToList();
            return View(items);
        }

        public IActionResult ListComment(int id)
        {
            var items = _context.PostComments.Where(i => i.PostId == id).OrderByDescending(i => i.PostCommentId).ToList();
            ViewBag.Account = _context.Accounts.Where(i => i.RoleID == 3).ToList();
            return View(items);
        }

        public IActionResult Details(int id)
        {
            var item = _context.PostComments.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewBag.Account = _context.Accounts.FirstOrDefault(i => i.AccountId == item.AccountId);
            return View(item);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.PostComments.Find(id);
            if (item != null)
            {
                _context.PostComments.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
