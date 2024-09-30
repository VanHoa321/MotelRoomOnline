using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;
using WebMyPhamOnline.Infrastructure;

namespace MotelRoomOnline.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        public LoginController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Account account)
        {
            if (account == null)
            {
                return NotFound();
            }
            string pw = Functions.MD5Password(account.Password);
            var check = _context.Accounts.Where(a => (a.AccountName == account.AccountName) && (a.Password == pw)).FirstOrDefault();
            if (check == null)
            {
                Functions.message = "Tài khoản trên không tồn tại!";
                return RedirectToAction("Index", "Login");
            }
            if (account.Status == false)
            {
                Functions.message = "Tài khoản đã bị khóa!";
                return RedirectToAction("Index", "Login");
            }
            Functions.account = check;
            Functions.message = string.Empty;
            switch (check.RoleID)
            {
                case 1:

                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                case 2:
                    return RedirectToAction("Index", "Home", new { area = "Landlord" });
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
    }
}
