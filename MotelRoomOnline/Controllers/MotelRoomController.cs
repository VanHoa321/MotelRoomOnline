using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;

namespace MotelRoomOnline.Controllers
{
    public class MotelRoomController : Controller
    {
        private readonly DataContext _context;
        public int pageSize = 6;
        public MotelRoomController(DataContext context)
        {
            _context = context;
        }

        [Route("/phong-tro")]
        public IActionResult Index()
        {
            ViewBag.RCategory = _context.RoomCategories.ToList();
            ViewBag.Ward = _context.Wards.ToList();
            return View();
        }

        public IActionResult GetData(int page = 1, string searchKey = "", int categoryId = 0, int wardId = 0)
        {
            var query = _context.Rooms.Where(i => i.RoomStatusId == 1);
            if (!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(i => i.RoomName.Contains(searchKey) || i.Abstract.Contains(searchKey));
            }
            if (categoryId > 0)
            {
                query = query.Where(i => i.RoomCategoryId == categoryId);
            }
            if (wardId > 0)
            {
                query = query.Where(i => i.WardId == wardId);
            }
            var room = query.OrderByDescending(i => i.RoomId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var roomListViewModel = new RoomListViewModel
            {
                Rooms = room,
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = query.Count()
                }
            };
            return Json(new { data = roomListViewModel });
        }

        [Route("/phong-tro/{alias}-{id}.html")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = _context.Rooms.FirstOrDefault(i => (i.RoomId == id) && (i.RoomStatusId == 1));

            if (room == null)
            {
                return NotFound();
            }
            room.ViewCount += 1;
            _context.SaveChanges();
            ViewBag.Account = _context.Accounts.FirstOrDefault(i => i.AccountId == room.AccountId);
            ViewBag.Image = _context.RoomImages.Where(i => (i.RoomId == room.RoomId) && (i.IsDefault == true)).ToList();
            return View(room);
        }
    }
}
