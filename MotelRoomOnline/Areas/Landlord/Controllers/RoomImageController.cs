using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class RoomImageController : Controller
    {
        private readonly DataContext _context;

        public RoomImageController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(long id)
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            if (id == 0)
            {
                return NotFound();
            }
            Functions.RoomId = id;
            var items = _context.RoomImages.Where(r => r.RoomId == id).OrderByDescending(r => r.RoomImageId).ToList();
            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RoomImage create)
        {
            if (ModelState.IsValid)
            {
                create.RoomId = Functions.RoomId;
                _context.Add(create);
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = Functions.RoomId });
            }
            return View(create);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _context.RoomImages.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(RoomImage edit)
        {
            if (ModelState.IsValid)
            {
                edit.RoomId = Functions.RoomId;
                _context.RoomImages.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = Functions.RoomId });
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.RoomImages.Find(id);
            if (item != null)
            {
                _context.RoomImages.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsDefault(int id)
        {
            var item = _context.RoomImages.Find(id);
            if (item != null)
            {
                item.IsDefault = !item.IsDefault;
                _context.SaveChanges();
                return Json(new { success = true, isDefault = item.IsDefault });
            }
            return Json(new { success = false });
        }

        public IActionResult GetData()
        {
            var items = _context.RoomImages.Where(r => r.RoomId == Functions.RoomId).OrderByDescending(r => r.RoomImageId).Take(10).ToList();
            return Json(new { data = items, totalItems = items.Count });
        }
    }
}
