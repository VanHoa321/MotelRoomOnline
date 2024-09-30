using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Areas.Admin.Models;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;
using WebMyPhamOnline.Infrastructure;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LandlordMenuController : Controller
    {
        private readonly DataContext _context;

        public LandlordMenuController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.AdminMenus.Where(m => m.AreaName == ("Landlord")).OrderByDescending(m => m.AdminMenuId).ToList();
            ViewBag.List = _context.AdminMenus.ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            var mnList = (from m in _context.AdminMenus.Where(m => (m.IsActive == true) && (m.AreaName == ("Landlord")) && (m.ItemLevel == 1))
                          select new SelectListItem()
                          {
                              Text = m.ItemName,
                              Value = m.AdminMenuId.ToString(),
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
        public IActionResult Create(AdminMenu create)
        {
            if (ModelState.IsValid)
            {
                _context.AdminMenus.Add(create);
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
            var item = _context.AdminMenus.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            var mnList = (from m in _context.AdminMenus.Where(m => (m.IsActive == true) && (m.AreaName == ("Landlord")) && (m.ItemLevel == 1))
                          select new SelectListItem()
                          {
                              Text = m.ItemName,
                              Value = m.AdminMenuId.ToString(),
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
        public IActionResult Edit(AdminMenu edit)
        {
            if (ModelState.IsValid)
            {
                _context.AdminMenus.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index", "LandlordMenu");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.AdminMenus.Find(id);
            if (item != null)
            {
                _context.AdminMenus.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int? id)
        {
            var item = _context.AdminMenus.Find(id);
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
            var items = _context.AdminMenus.Where(m => m.AreaName == ("Landlord")).OrderByDescending(m => m.AdminMenuId).Take(10).ToList();
            return Json(new { data = items, totalItems = items.Count });
        }
    }
}
