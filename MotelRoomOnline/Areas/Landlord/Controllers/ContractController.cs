using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class ContractController : Controller
    {
        private readonly DataContext _context;

        public ContractController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            var accountId = Functions.account.AccountId;
            var items = (from contract in _context.Contracts join room in _context.Rooms
                         on contract.RoomId equals room.RoomId
                         where room.AccountId == accountId && contract.Status == 1
                         orderby contract.RoomId descending
                         select contract).ToList();

            var listCustomer = _context.Customers.Where(c => c.AccountId == Functions.account.AccountId).ToList();
            var listRoom = _context.Rooms.Where(r => r.AccountId == Functions.account.AccountId).ToList();
            ViewBag.listCustomer = listCustomer;
            ViewBag.listRoom = listRoom;
            return View(items);
        }

        public IActionResult Detail(long id)
        {
            Functions.DetailId = id;
            if (id == 0)
            {
                return NotFound();
            }
            Functions.ContractId = id;
            var item = _context.Contracts.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            Functions.RoomId = item.RoomId;
            ViewBag.Room = _context.Rooms.FirstOrDefault(r => r.RoomId == item.RoomId);
            ViewBag.Customer = _context.Customers.FirstOrDefault(c => c.CustomerId == item.CustomerId);
            return View(item);
        }

        [HttpPost]
        public IActionResult AddRentalMonth()
        {
            var contract = _context.Contracts.Find(Functions.ContractId);
            if (contract == null)
            {
                return NotFound();
            }

            var lastRentalMonth = _context.RentalMonths.OrderByDescending(r => r.EndDate).FirstOrDefault(r => r.ContractId == contract.ContractId);

            DateTime startDate;
            DateTime endDate;

            if (lastRentalMonth == null)
            {
                startDate = contract.CreatedDate;
            }
            else
            {
                startDate = lastRentalMonth.EndDate.AddDays(1);
            }

            endDate = startDate.AddDays(30);
            var rentalMonth = new RentalMonth
            {
                ContractId = contract.ContractId,
                StartDate = startDate,
                EndDate = endDate,
                PriceRoom = contract.PriceRoom,
                Status = 1
            };

            _context.RentalMonths.Add(rentalMonth);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
