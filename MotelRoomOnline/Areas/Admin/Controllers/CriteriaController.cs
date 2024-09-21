using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CriteriaController : Controller
    {
        private readonly DataContext _context;

        public CriteriaController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Criterias.OrderByDescending(c => c.CriteriaId).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Criteria create)
        {
            if (ModelState.IsValid)
            {
                _context.Criterias.Add(create);
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
            var item = _context.Criterias.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Criteria edit)
        {
            if (ModelState.IsValid)
            {
                _context.Criterias.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.Criterias.Find(id);
            if (item != null)
            {
                _context.Criterias.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int? id)
        {
            var item = _context.Criterias.Find(id);
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
            var items = _context.Criterias.OrderByDescending(c => c.CriteriaId).ToList();
            return Json(new { data = items, totalItems = items.Count });
        }
    }
}
