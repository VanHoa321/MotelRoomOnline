using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Services;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class PremiumController : Controller
    {
        private readonly DataContext _context;
        private readonly IVnPayService _vpnPayService;

        public PremiumController(DataContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vpnPayService = vnPayService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upgrade(int id)
        {
            Functions.PackageId = id;
            var package = _context.packages.Find(id);
            if (package != null)
            {
                var model = new VnPaymentRequestModel
                {
                    FullName = Functions.account.FullName,
                    OrderId = new Random().Next(1000, 10000),
                    Description = "Chức năng VnPay",
                    Amount = package.Price,
                    CreatedDate = DateTime.Now,
                    PackageId = package.PackageId,
                    PackageName = package.Name,
                };
                return Redirect(_vpnPayService.CreatePaymentUrl(HttpContext, model));
            }
            return NotFound();
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction("Index");
            }
            var account = _context.Accounts.Find(Functions.account.AccountId);
            var package = _context.packages.Find(Functions.PackageId);
            account.PremiumId = Functions.PackageId;

            if (account.ExpiryDate == null || account.ExpiryDate <= DateTime.Now)
            {
                // Nếu ExpiryDate là null hoặc đã hết hạn, set ExpiryDate từ ngày hiện tại
                account.ExpiryDate = DateTime.Now.AddDays(package.DurationDays);
            }
            else
            {
                // Nếu ExpiryDate còn hạn, cộng thêm số ngày vào ngày hết hạn hiện tại
                account.ExpiryDate = account.ExpiryDate.Value.AddDays(package.DurationDays);
            }
            var accountUpgrade = new AccountUpgrade
            {
                AccountId = account.AccountId,
                PackageId = Functions.PackageId,
                Amount = package.Price,
                UpgradeDate = DateTime.Now
            };
            _context.accountUpgrades.Add(accountUpgrade);
            _context.SaveChanges();
            return RedirectToAction("PaymentSuccess");
        }

        public IActionResult PaymentSuccess()
        {
            var package = _context.packages.Find(Functions.PackageId);
            return View(package);
        }
    }
}
