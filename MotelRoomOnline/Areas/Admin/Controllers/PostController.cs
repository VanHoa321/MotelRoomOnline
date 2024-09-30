using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly DataContext _context;

        public PostController(DataContext context)
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

        public IActionResult Create()
        {
            var list = (from p in _context.PostCategories.Where(p => (p.IsActive == true))
                          select new SelectListItem()
                          {
                              Text = p.PostCategoryName,
                              Value = p.PostCategoryId.ToString(),
                          }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Text = "---Chọn---",
                Value = "0"
            });
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post create)
        {
            if (ModelState.IsValid)
            {
                create.Alias = Functions.TitleSlugGenerationAlias(create.PostTitle);
                create.AccountId = Functions.account.AccountId;
                _context.Posts.Add(create);
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
            var item = _context.Posts.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            var list = (from p in _context.PostCategories.Where(p => (p.IsActive == true))
                        select new SelectListItem()
                        {
                            Text = p.PostCategoryName,
                            Value = p.PostCategoryId.ToString(),
                        }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Text = "---Chọn---",
                Value = "0"
            });
            ViewBag.list = list;
            return View(item);
        }

        public IActionResult Detail(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _context.Posts.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            var list = (from p in _context.PostCategories.Where(p => (p.IsActive == true))
                        select new SelectListItem()
                        {
                            Text = p.PostCategoryName,
                            Value = p.PostCategoryId.ToString(),
                        }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Text = "---Chọn---",
                Value = "0"
            });
            ViewBag.list = list;
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Post edit)
        {
            if (ModelState.IsValid)
            {
                edit.Alias = Functions.TitleSlugGenerationAlias(edit.PostTitle);
                edit.AccountId = Functions.account.AccountId;
                _context.Posts.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.Posts.Find(id);
            if (item != null)
            {
                _context.Posts.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int? id)
        {
            var item = _context.Posts.Find(id);
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
            var items = _context.Posts.OrderByDescending(p => p.PostId).Take(10).ToList();
            var listC = _context.PostCategories.ToList();
            return Json(new { data = items, totalItems = items.Count, listCategory = listC });
        }
    }
}
