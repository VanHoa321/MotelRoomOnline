using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;
using MotelRoomOnline.Models.ViewModels;
using Newtonsoft.Json;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            ViewBag.UserCount = _context.Customers.Where(i => i.AccountId == Functions.account.AccountId).Count();
            ViewBag.MotelCount = _context.Rooms.Where(i => i.AccountId == Functions.account.AccountId).Count();
            ViewBag.PostCount = _context.Posts.Where(i => i.AccountId == Functions.account.AccountId).Count();
            ViewBag.ContractCount = _context.Rooms.Where(i => (i.AccountId == Functions.account.AccountId) && (i.RoomStatusId == 3)).Count();

            var topRooms = _context.Rooms
                .Where(r => r.AccountId == Functions.account.AccountId)
                .Select(r => new DashboardViewModel
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    Image = r.Image,
                    OwnerName = _context.Accounts.FirstOrDefault(a => a.AccountId == r.AccountId).FullName,
                    TotalRevenue = _context.Contracts
                                        .Where(c => c.RoomId == r.RoomId)
                                        .Join(_context.RentalMonths,
                                              c => c.ContractId,
                                              rm => rm.ContractId,
                                              (c, rm) => rm)
                                        .Join(_context.Invoices,
                                              rm => rm.RentalMonthId,
                                              i => i.RentalMonthId,
                                              (rm, i) => i.TotalAmount)
                                        .Sum()
                })
                .Where(r => r.TotalRevenue > 0)
                .OrderByDescending(r => r.TotalRevenue)
                .Take(5)
                .ToList();

            var currentYear = DateTime.Now.Year;
            var monthlyStatistics = (from invoice in _context.Invoices
                                     join contract in _context.Contracts
                                         on invoice.ContractId equals contract.ContractId
                                     join customer in _context.Customers
                                         on contract.CustomerId equals customer.CustomerId
                                     where invoice.InvoiceDate.Year == currentYear &&
                                           customer.AccountId == Functions.account.AccountId
                                     group invoice by invoice.InvoiceDate.Month into grouped
                                     select new
                                     {
                                         Month = grouped.Key,
                                         TotalAmount = grouped.Sum(i => i.TotalAmount)
                                     })
                      .OrderBy(m => m.Month)
                      .ToList();

            var statistics = Enumerable.Range(1, 12).Select(month => new
            {
                Month = month,
                TotalAmount = monthlyStatistics.FirstOrDefault(m => m.Month == month)?.TotalAmount ?? 0
            }).ToList();

            ViewBag.TopRooms = JsonConvert.SerializeObject(topRooms);
            ViewBag.Statistics = JsonConvert.SerializeObject(statistics);
            return View(topRooms);
        }

        public IActionResult GetRoomInvoices(int roomId)
        {
            var invoices = (from invoice in _context.Invoices
                            join rentalMonth in _context.RentalMonths on invoice.RentalMonthId equals rentalMonth.RentalMonthId
                            join contract in _context.Contracts on rentalMonth.ContractId equals contract.ContractId
                            join room in _context.Rooms on contract.RoomId equals room.RoomId
                            join customer in _context.Customers on contract.CustomerId equals customer.CustomerId
                            where room.RoomId == roomId
                            select new
                            {
                                Invoice = invoice,       
                                RentalMonth = rentalMonth,
                                Contract = contract,
                                Customer = customer
                            }).ToList();

            return Json(new { data = invoices });
        }

        public IActionResult GetInvoiceDetails(int rentalMonthId)
        {
            var invoiceDetails = (from contractDetail in _context.ContractDetails
                                  join rentalMonth in _context.RentalMonths on contractDetail.RentalMonthId equals rentalMonth.RentalMonthId
                                  join roomService in _context.RoomServices on contractDetail.RoomServiceId equals roomService.RoomServiceId
                                  join service in _context.Services on roomService.ServiceId equals service.ServiceId
                                  join invoice in _context.Invoices on rentalMonth.RentalMonthId equals invoice.RentalMonthId
                                  where rentalMonth.RentalMonthId == rentalMonthId
                                  select new
                                  {
                                      Name = service.ServiceName,
                                      Price = contractDetail.ServicePrice,
                                      Quantity = contractDetail.ServiceQuantity,
                                      Total = contractDetail.Amount,
                                      RoomPrice = rentalMonth.PriceRoom,
                                      Start = rentalMonth.StartDate,
                                      End = rentalMonth.EndDate,
                                      Amount = invoice.TotalAmount
                                  }).ToList();

            return Json(new { data = invoiceDetails });
        }

        public IActionResult GetTopCustomers()
        {
            var topCustomers = _context.Customers
                .Where(c => c.AccountId == Functions.account.AccountId)
                .Select(c => new
                {
                    CusId = c.CustomerId,
                    Name = c.FullName,
                    CCCD = c.Code,
                    Avt = c.Avatar,
                    TotalRevenue = _context.Contracts
                        .Where(ct => ct.CustomerId == c.CustomerId)
                        .Join(_context.RentalMonths,
                              ct => ct.ContractId,
                              rm => rm.ContractId,
                              (ct, rm) => rm)
                        .Join(_context.Invoices,
                              rm => rm.RentalMonthId,
                              i => i.RentalMonthId,
                              (rm, i) => i.TotalAmount)
                        .Sum()
                })
                .Where(c => c.TotalRevenue > 0)
                .OrderByDescending(c => c.TotalRevenue)
                .ToList();

            var topCustomersLimit5 = _context.Customers
                .Where(c => c.AccountId == Functions.account.AccountId)
                .Select(c => new
                {
                    CusId = c.CustomerId,
                    Name = c.FullName,
                    CCCD = c.Code,
                    Avt = c.Avatar,
                    TotalRevenue = _context.Contracts
                        .Where(ct => ct.CustomerId == c.CustomerId)
                        .Join(_context.RentalMonths,
                              ct => ct.ContractId,
                              rm => rm.ContractId,
                              (ct, rm) => rm)
                        .Join(_context.Invoices,
                              rm => rm.RentalMonthId,
                              i => i.RentalMonthId,
                              (rm, i) => i.TotalAmount)
                        .Sum()
                })
                .Where(c => c.TotalRevenue > 0)
                .OrderByDescending(c => c.TotalRevenue)
                .Take(5)
                .ToList();

            return Json(new { data = topCustomers, data5 = topCustomersLimit5 });
        }

        public IActionResult GetCusInvoices(int cusId)
        {
            var invoices = (from invoice in _context.Invoices
                            join rentalMonth in _context.RentalMonths on invoice.RentalMonthId equals rentalMonth.RentalMonthId
                            join contract in _context.Contracts on rentalMonth.ContractId equals contract.ContractId
                            join room in _context.Rooms on contract.RoomId equals room.RoomId
                            join customer in _context.Customers on contract.CustomerId equals customer.CustomerId
                            where customer.CustomerId == cusId
                            select new
                            {
                                Invoice = invoice,
                                RentalMonth = rentalMonth,
                                Contract = contract,
                                Room = room,
                                Cus = customer
                            }).ToList();

            return Json(new { data = invoices });
        }

        public IActionResult GetMonthlyCustomerCount()
        {
            var currentYear = DateTime.Now.Year;

            // Lấy số tháng thuê cho từng tháng trong năm hiện tại
            var monthlyRentalStatistics = (from rentalMonth in _context.RentalMonths
                                           join contract in _context.Contracts
                                               on rentalMonth.ContractId equals contract.ContractId
                                           join customer in _context.Customers
                                               on contract.CustomerId equals customer.CustomerId
                                           where rentalMonth.StartDate.Year == currentYear &&
                                                 customer.AccountId == Functions.account.AccountId
                                           group rentalMonth by rentalMonth.StartDate.Month into grouped
                                           select new
                                           {
                                               Month = grouped.Key,
                                               TotalRentalMonths = grouped.Count()
                                           })
                                .OrderBy(m => m.Month)
                                .ToList();

            var statistics = Enumerable.Range(1, 12).Select(month => new
            {
                Month = month,
                TotalRentalMonths = monthlyRentalStatistics.FirstOrDefault(m => m.Month == month)?.TotalRentalMonths ?? 0
            }).ToList();
            return Json(new { data = monthlyRentalStatistics });
        }

    }
}
