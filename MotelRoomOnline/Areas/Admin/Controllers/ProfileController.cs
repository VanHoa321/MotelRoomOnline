using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;
using System.Security.Principal;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfileController : Controller
    {
        private readonly DataContext _context;

        public ProfileController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var item = _context.Accounts.Find(Functions.account?.AccountId);
            return View(item);
        }

        public IActionResult EditAccount()
        {
            var item = _context.Accounts.Find(Functions.account?.AccountId);
            return View(item);
        }

        [HttpPost]
        public IActionResult EditAccount(Account edit)
        {
            if (ModelState.IsValid)
            {
                bool checkEmail = _context.Accounts.Any(a => a.Email == edit.Email && a.AccountId != edit.AccountId);
                if (checkEmail)
                {
                    Functions.message = "Email này đã tồn tại!";
                    return View(edit);
                }
                bool checkPhone = _context.Accounts.Any(a => a.Phone == edit.Phone && a.AccountId != edit.AccountId);
                if (checkPhone)
                {
                    Functions.message = "Số điện thoại này đã tồn tại!";
                    return View(edit);
                }
                Functions.message = string.Empty;
                _context.Accounts.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        public IActionResult EditPassWord()
        {
            var item = _context.Accounts.Find(Functions.account?.AccountId);
            return View(item);
        }

        [HttpPost]
        public IActionResult EditPassWord(Account edit, string NewPassword)
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.Find(edit.AccountId);
                if (account != null && Functions.MD5Password(edit.Password) == account.Password)
                {
                    account.Password = Functions.MD5Password(NewPassword);
                    _context.Accounts.Update(account);
                    _context.SaveChanges();
                    Functions.message = "Đổi mật khẩu thành công!";
                }
                else
                {
                    Functions.message = "Nhập sai mật khẩu cũ!";
                }
                return RedirectToAction("EditPassWord");
            }
            return View(edit);
        }
    }
}
