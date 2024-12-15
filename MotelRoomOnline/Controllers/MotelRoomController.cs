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

        public IActionResult GetData(int page = 1, string searchKey = "", int categoryId = 0, int wardId = 0, decimal minPrice = 0, decimal maxPrice = 0, int minAcreage = 0, int maxAcreage = 0)
        {
            var query = _context.Rooms
            .Join(
                _context.Accounts,
                room => room.AccountId,
                account => account.AccountId,
                (room, account) => new { Room = room, PremiumId = account.PremiumId }
            )
            .Where(x => x.Room.RoomStatusId == 1);

            if (!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(x => x.Room.RoomName.Contains(searchKey) || x.Room.Abstract.Contains(searchKey));
            }

            if (categoryId > 0)
            {
                query = query.Where(x => x.Room.RoomCategoryId == categoryId);
            }

            if (wardId > 0)
            {
                query = query.Where(x => x.Room.WardId == wardId);
            }

            if (minPrice > 0)
            {
                query = query.Where(x => x.Room.PriceRoom >= minPrice);
            }
            if (maxPrice > 0)
            {
                query = query.Where(x => x.Room.PriceRoom <= maxPrice);
            }

            if (minAcreage > 0)
            {
                query = query.Where(x => x.Room.Acreage >= minAcreage);
            }

            if (maxAcreage > 0)
            {
                query = query.Where(x => x.Room.Acreage <= maxAcreage);
            }

            var roomList = query
                .OrderByDescending(x => x.PremiumId) // Sắp xếp theo PremiumId của chủ trọ
                .ThenByDescending(x => x.Room.RoomId) // Sắp xếp tiếp theo RoomId
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.Room)
                .ToList();

            var roomListViewModel = new RoomListViewModel
            {
                Rooms = roomList,
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
            ViewBag.RelatedMotel = _context.Rooms.Where(i => (i.AccountId == room.AccountId) || (i.RoomCategoryId == room.RoomCategoryId) || (i.WardId == i.WardId) || (i.RoomStatusId == 1)).ToList();
            ViewBag.RoomService = _context.RoomServices.Where(i => (i.RoomId == id) && (i.IsActive == true)).ToList();
            ViewBag.RoomCriteria = _context.RoomCriterias.Where(i => (i.RoomId == id) && (i.IsActive == true)).ToList();
            ViewBag.Service = _context.Services.ToList();
            ViewBag.Criteria = _context.Criterias.ToList();
            return View(room);
        }
    }
}
