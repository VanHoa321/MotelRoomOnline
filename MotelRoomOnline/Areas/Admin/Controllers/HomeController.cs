using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;
using MotelRoomOnline.Utilities;
using Newtonsoft.Json;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            ViewBag.UserCount = _context.Accounts.Where(i => i.RoleID == 2).Count();
            ViewBag.MotelCount = _context.Rooms.Where(i => i.RoomStatusId == 5).Count();
            ViewBag.PostCount = _context.Posts.Where(i => i.IsActive == false).Count();
            ViewBag.LandlordCount = _context.Accounts.Where(i => i.RoleID == 4).Count();

            var topRooms = (from r in _context.Rooms
                            join a in _context.Accounts on r.AccountId equals a.AccountId
                            select new DashboardViewModel
                            {
                                RoomId = r.RoomId,
                                RoomName = r.RoomName,
                                Image = r.Image,
                                PhoneLand = a.Phone,
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
            var monthlyStatistics = _context.Invoices.Where(i => i.InvoiceDate.Year == currentYear).GroupBy(i => i.InvoiceDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(i => i.TotalAmount)
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
            var monthlyRentalStatistics = _context.RentalMonths
                .Where(rm => rm.StartDate.Year == currentYear)
                .GroupBy(rm => rm.StartDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalRentalMonths = g.Count()
                })
                .OrderBy(m => m.Month)
                .ToList();

            // Tạo danh sách cho tất cả 12 tháng
            var statistics = Enumerable.Range(1, 12).Select(month => new
            {
                Month = month,
                TotalRentalMonths = monthlyRentalStatistics.FirstOrDefault(m => m.Month == month)?.TotalRentalMonths ?? 0
            }).ToList();
            return Json(new { data = monthlyRentalStatistics });
        }

        public IActionResult GetTopLandlord()
        {
            var topLandlords = _context.Accounts
                .Where(a => a.RoleID == 2)
                .Join(
                    _context.accountUpgrades,
                    account => account.AccountId,
                    upgrade => upgrade.AccountId,
                    (account, upgrade) => new { account, upgrade.Amount }
                )
                .GroupBy(x => new { x.account.AccountId, x.account.FullName, x.account.Phone, x.account.Avatar })
                .Select(g => new
                {
                    AccountId = g.Key.AccountId,
                    FullName = g.Key.FullName,
                    PhoneNumber = g.Key.Phone,
                    Avatar = g.Key.Avatar,
                    TotalAmountSpent = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalAmountSpent)
                .Take(10)
                .ToList();

            return Json(new { data = topLandlords });
        }

        public IActionResult GetLandlordPackageHistory(int landlordId)
        {
            var packageHistory = _context.accountUpgrades
                .Where(upgrade => upgrade.AccountId == landlordId)
                .Join(
                    _context.packages,
                    upgrade => upgrade.PackageId,
                    package => package.PackageId,
                    (upgrade, package) => new
                    {
                        PackageName = package.Name,
                        Amount = upgrade.Amount,
                        PurchaseDate = upgrade.UpgradeDate
                    }
                )
                .OrderByDescending(x => x.PurchaseDate)
                .ToList();

            return Json(new { data = packageHistory });
        }

        public IActionResult GetPackagePurchaseStatistics()
        {
            var packageStatistics = _context.accountUpgrades
                .Join(
                    _context.packages,
                    upgrade => upgrade.PackageId,
                    package => package.PackageId,
                    (upgrade, package) => new
                    {
                        PackageName = package.Name,
                        Amount = upgrade.Amount
                    }
                )
                .GroupBy(x => x.PackageName)
                .Select(g => new
                {
                    PackageName = g.Key,
                    TotalAmountSpent = g.Sum(x => x.Amount),
                    TotalQuantity = g.Count()
                })
                .ToList();

            return Json(new { data = packageStatistics });
        }

        public IActionResult GetTopLandlordsByRevenue()
        {
            var topLandlords = _context.Accounts
                .Where(a => a.RoleID == 2)
                .Join(
                    _context.Customers,
                    account => account.AccountId,
                    customer => customer.AccountId,
                    (account, customer) => new { account, customer }
                )
                .Join(
                    _context.Contracts,
                    ac => ac.customer.CustomerId,
                    contract => contract.CustomerId,
                    (ac, contract) => new { ac.account, contract }
                )
                .Join(
                    _context.Invoices,
                    ac => ac.contract.ContractId,
                    invoice => invoice.ContractId,
                    (ac, invoice) => new { ac.account, invoice.TotalAmount }
                )
                .GroupBy(x => new { x.account.AccountId, x.account.FullName, x.account.Phone })
                .Select(g => new
                {
                    LandlordId = g.Key.AccountId,
                    LandlordName = g.Key.FullName,
                    LandlordPhone = g.Key.Phone,
                    TotalRevenue = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(10)
                .ToList();
            return Json(new { data = topLandlords });
        }

        public IActionResult GetMonthlyRevenueForCurrentYear()
        {
            var currentYear = DateTime.Now.Year;

            var monthlyRevenue = _context.accountUpgrades
                .Where(u => u.UpgradeDate.Year == currentYear)
                .GroupBy(u => u.UpgradeDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(u => u.Amount)
                })
                .OrderBy(g => g.Month)
                .ToList();

            var allMonths = Enumerable.Range(1, 12).Select(m => new
            {
                Month = m,
                TotalRevenue = monthlyRevenue.FirstOrDefault(x => x.Month == m)?.TotalRevenue ?? 0
            }).ToList();

            return Json(new { data = allMonths });
        }

        public IActionResult Logout()
        {
            Functions.account = null;
            Functions.message = string.Empty;
            return Redirect("/Home");
        }  
    }
}
