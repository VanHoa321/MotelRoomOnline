using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomCategoriesController : Controller
    {
        private readonly DataContext _context;

        public RoomCategoriesController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.RoomCategories.OrderByDescending(r => r.RoomCategoryId).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RoomCategories create)
        {
            if (ModelState.IsValid)
            {
                create.Alias = Functions.TitleSlugGenerationAlias(create.RoomCategoryName);
                _context.RoomCategories.Add(create);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(create);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _context.RoomCategories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(RoomCategories edit)
        {
            if (ModelState.IsValid)
            {
                edit.Alias = Functions.TitleSlugGenerationAlias(edit.RoomCategoryName);
                _context.RoomCategories.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.RoomCategories.Find(id);
            if (item != null)
            {
                _context.RoomCategories.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int? id)
        {
            var item = _context.RoomCategories.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _context.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        public IActionResult GetData()
        {
            var items = _context.RoomCategories.OrderByDescending(r => r.RoomCategoryId).Take(10).ToList();
            return Json(new { data = items, totalItems = items.Count });
        }
    }
}
