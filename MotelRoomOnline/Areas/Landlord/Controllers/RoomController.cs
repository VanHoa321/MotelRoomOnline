using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class RoomController : Controller
    {
        private readonly DataContext _context;

        public RoomController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            ViewBag.roomStatus = _context.RoomStatuses.ToList();
            var items = _context.Rooms.Where(r => r.AccountId == Functions.account.AccountId).OrderByDescending(r => r.RoomId).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            var listWard = (from w in _context.Wards
                            select new SelectListItem()
                            {
                                Text = w.WardName,
                                Value = w.WardId.ToString(),
                            }).ToList();
            listWard.Insert(0, new SelectListItem()
            {
                Text = "Chọn Phường/Xã",
                Value = "0"
            });

            var listRoomCategories = (from r in _context.RoomCategories.Where(r => r.IsActive == true)
                                      select new SelectListItem()
                                      {
                                          Text = r.RoomCategoryName,
                                          Value = r.RoomCategoryId.ToString(),
                                      }).ToList();
            listRoomCategories.Insert(0, new SelectListItem()
            {
                Text = "Chọn phân loại",
                Value = "0"
            });

            var listRoomStatus = (from r in _context.RoomStatuses.Where(r => r.IsActive == true)
                                  select new SelectListItem()
                                  {
                                      Text = r.RoomStatusName,
                                      Value = r.RoomStatusId.ToString(),
                                  }).ToList();
            listRoomStatus.Insert(0, new SelectListItem()
            {
                Text = "Chọn trạng thái",
                Value = "0"
            });
            ViewBag.listWard = listWard;
            ViewBag.listRoomCategories = listRoomCategories;
            ViewBag.listRoomStatus = listRoomStatus;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room create)
        {
            if (ModelState.IsValid)
            {
                create.AccountId = Functions.account.AccountId;
                _context.Rooms.Add(create);
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

            var item = _context.Rooms.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            var listWard = (from w in _context.Wards
                            select new SelectListItem()
                            {
                                Text = w.WardName,
                                Value = w.WardId.ToString(),
                            }).ToList();
            listWard.Insert(0, new SelectListItem()
            {
                Text = "Chọn Phường/Xã",
                Value = "0"
            });

            var listRoomCategories = (from r in _context.RoomCategories.Where(r => r.IsActive == true)
                                      select new SelectListItem()
                                      {
                                          Text = r.RoomCategoryName,
                                          Value = r.RoomCategoryId.ToString(),
                                      }).ToList();
            listRoomCategories.Insert(0, new SelectListItem()
            {
                Text = "Chọn phân loại",
                Value = "0"
            });

            var listRoomStatus = (from r in _context.RoomStatuses.Where(r => r.IsActive == true)
                                  select new SelectListItem()
                                  {
                                      Text = r.RoomStatusName,
                                      Value = r.RoomStatusId.ToString(),
                                  }).ToList();
            listRoomStatus.Insert(0, new SelectListItem()
            {
                Text = "Chọn trạng thái",
                Value = "0"
            });
            ViewBag.listWard = listWard;
            ViewBag.listRoomCategories = listRoomCategories;
            ViewBag.listRoomStatus = listRoomStatus;
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Room edit)
        {
            if (ModelState.IsValid)
            {
                edit.AccountId = Functions.account.AccountId;
                _context.Rooms.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.Rooms.Find(id);
            if (item != null)
            {
                _context.Rooms.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult GetData()
        {
            var items = _context.Rooms.Where(r => r.AccountId == Functions.account.AccountId).OrderByDescending(r => r.RoomId).ToList();
            var roomS = _context.RoomStatuses.ToList();
            return Json(new { data = items, totalItems = items.Count, roomStatus = roomS });
        }
    }
}
