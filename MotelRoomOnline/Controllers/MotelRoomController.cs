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
        public IActionResult Index(int page = 1)
        {
            var room = _context.Rooms.Where(i => i.RoomStatusId == 1).OrderByDescending(i => i.RoomId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Account = _context.Accounts.ToList();
            var roomListViewModel = new RoomListViewModel
            {
                Rooms = room,
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = _context.Rooms.Where(i => i.RoomStatusId == 1).Count()
                }
            };
            return View(roomListViewModel);
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

            ViewBag.Account = _context.Accounts.FirstOrDefault(i => i.AccountId == room.AccountId);
            ViewBag.Image = _context.RoomImages.Where(i => (i.RoomId == room.RoomId) && (i.IsDefault == true)).ToList();
            return View(room);
        }
    }
}
