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
            Functions.message = string.Empty;
            switch (check.RoleID)
            {
                case 1:
                    Functions.account = check;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                case 2:
                    Functions.account = check;
                    return RedirectToAction("Index", "Post", new { area = "Admin" });
                case 3:
                    Functions.account = check;
                    return RedirectToAction("Index", "Post", new { area = "Admin" });
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
    }
}
