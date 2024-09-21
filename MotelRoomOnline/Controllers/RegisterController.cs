using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DataContext _context;
        public RegisterController(DataContext context)
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
            var check = _context.Accounts.Where(a => (a.Email == account.Email) || (a.AccountName == account.AccountName)).FirstOrDefault();
            if (check != null)
            {
                Functions.message = "Tên đăng nhập hoặc Email này đã được đăng ký!";
                return RedirectToAction("Index", "Register");
            }
            Functions.message = string.Empty;
            account.Password = Functions.MD5Password(account.Password);
            account.Phone = "0123456789";
            account.Status = true;
            account.RoleID = 3;
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}
