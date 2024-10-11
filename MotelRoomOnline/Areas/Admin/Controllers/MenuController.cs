using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Areas.Admin.Models;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly DataContext _context;

        public MenuController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Menus.OrderByDescending(m => m.MenuId).ToList();
            ViewBag.List = _context.Menus.Where(m => m.ParentId == 0).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            var mnList = (from m in _context.Menus.Where(m => (m.IsActive == true) && (m.Levels == 1))
                          select new SelectListItem()
                          {
                              Text = m.MenuName,
                              Value = m.MenuId.ToString(),
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "---Chọn---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Menu create)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Add(create);
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
            var item = _context.Menus.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            var mnList = (from m in _context.Menus.Where(m => (m.IsActive == true) && (m.Levels == 1))
                          select new SelectListItem()
                          {
                              Text = m.MenuName,
                              Value = m.MenuId.ToString(),
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "---Chọn---",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Menu edit)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.Menus.Find(id);
            if (item != null)
            {
                _context.Menus.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int? id)
        {
            var item = _context.Menus.Find(id);
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
            var items = _context.Menus.OrderByDescending(m => m.MenuId).Take(10).ToList();
            return Json(new { data = items, totalItems = items.Count });
        }
    }
}
