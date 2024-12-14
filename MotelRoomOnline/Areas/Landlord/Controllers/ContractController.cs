using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public IActionResult Contract()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            var accountId = Functions.account.AccountId;
            var items = (from contract in _context.Contracts
                         join room in _context.Rooms
                         on contract.RoomId equals room.RoomId
                         where room.AccountId == accountId && contract.Status == 0
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

        public IActionResult Create()
        {
            var rooms = (from r in _context.Rooms.Where(r =>r.RoomStatusId == 1)
                            select new SelectListItem()
                            {
                                Text = r.RoomName,
                                Value = r.RoomId.ToString(),
                            }).ToList();
            rooms.Insert(0, new SelectListItem()
            {
                Text = "--Chọn phòng--",
                Value = "0"
            });

            var customer = (from c in _context.Customers
                         select new SelectListItem()
                         {
                             Text = c.Phone + " - "+ c.FullName,
                             Value = c.CustomerId.ToString(),
                         }).ToList();
            customer.Insert(0, new SelectListItem()
            {
                Text = "--Chọn khách hàng--",
                Value = "0"
            });
            ViewBag.Rooms = rooms;
            ViewBag.Customers = customer;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contract create)
        {
            if (ModelState.IsValid)
            {
                var room = _context.Rooms.FirstOrDefault(r => r.RoomId == create.RoomId);
                if (room != null)
                {
                    room.RoomStatusId = 3;
                    _context.Contracts.Add(create);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Phòng không tồn tại.");
                }
            }
            return View(create);
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

        [HttpPost]
        public IActionResult CheckOut()
        {
            var contract = _context.Contracts.Find(Functions.ContractId);
            if (contract == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hợp đồng" });
            }

            var unpaidMonths = _context.RentalMonths.Where(r => r.ContractId == contract.ContractId && r.Status == 1).ToList();

            if (unpaidMonths.Count > 0)
            {
                return Json(new { success = false, message = "Có tháng thuê chưa được thanh toán" });
            }

            contract.Status = 0;
            contract.EndDate = DateTime.Now;
            var room = _context.Rooms.Find(contract.RoomId);
            if (room != null)
            {
                room.RoomStatusId = 1;
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Trả phòng trọ thành công" });
        }

        public IActionResult ContractView(long id)
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

        public IActionResult GetRoomById(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return Json(new
            {
                room.Image,
                room.RoomName,
                room.Address,
                room.Acreage,
                room.MaxPeople,
                room.PriceRoom
            });
        }

        public IActionResult GetCustomerById(long id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Json(new
            {
                customer.Avatar,
                customer.FullName,
                customer.Code,
                DOB = customer.DOB?.ToString("dd/MM/yyyy"),
                customer.Phone
            });
        }
    }
}
