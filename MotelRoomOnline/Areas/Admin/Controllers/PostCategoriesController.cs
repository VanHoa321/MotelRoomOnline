using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Areas.Admin.Models;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostCategoriesController : Controller
    {
        private readonly DataContext _context;

        public PostCategoriesController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return Redirect("/Login/Index");
            }
            var items = _context.PostCategories.OrderByDescending(p => p.PostCategoryId).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostCategories create)
        {
            if (ModelState.IsValid)
            {
                create.Alias = Functions.TitleSlugGenerationAlias(create.PostCategoryName);
                _context.PostCategories.Add(create);
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
            var item = _context.PostCategories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(PostCategories edit)
        {
            if (ModelState.IsValid)
            {
                edit.Alias = Functions.TitleSlugGenerationAlias(edit.PostCategoryName);
                _context.PostCategories.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.PostCategories.Find(id);
            if (item != null)
            {
                _context.PostCategories.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int? id)
        {
            var item = _context.PostCategories.Find(id);
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
            var items = _context.PostCategories.OrderByDescending(p => p.PostCategoryId).ToList();
            return Json(new { data = items, totalItems = items.Count });
        }
    }
}
