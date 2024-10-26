using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;
using System.Security.Principal;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        public IActionResult ListAdmin()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Accounts.Where(i => i.RoleID == 1).OrderByDescending(i => i.AccountId).ToList();
            return View(items);
        }

        public IActionResult ListLandlord()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Accounts.Where(i => i.RoleID == 2).ToList();
            return View(items);
        }

        public IActionResult ListCustomer()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Accounts.Where(i => i.RoleID == 3).ToList();
            return View(items);
        }

        public IActionResult Details(int id)
        {
            var item = _context.Accounts.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id)
        {
            var item = _context.Accounts.Find(id);
            if (item != null)
            {
                item.Status = !item.Status;
                _context.SaveChanges();
                return Json(new { success = true, status = item.Status });
            }
            return Json(new { success = false });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Account create)
        {
            if (create == null)
            {
                return NotFound();
            }
            var check = _context.Accounts.Where(a => (a.Email == create.Email) || (a.AccountName == create.AccountName)).FirstOrDefault();
            if (check != null)
            {
                Functions.message = "Tên đăng nhập hoặc Email này đã được đăng ký!";
                return View(create);
            }
            Functions.message = string.Empty;
            create.Password = Functions.MD5Password(create.Password);
            create.Phone = "0123456789";
            create.Status = true;
            create.Avatar = "/admin/assets/dist/img/avatar.png";
            create.DOB = DateTime.Now;
            create.RoleID = 1;
            create.CreatedDate = DateTime.Now;
            _context.Accounts.Add(create);
            _context.SaveChanges();
            return RedirectToAction("ListAdmin", "User");
        }

        [HttpPost]
        public IActionResult RegisterLandlord(int id)
        {
            var item = _context.Accounts.Find(id);
            if (item != null)
            {
                item.RoleID = 4;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult ListRegister()
        {
            if (!Functions.IsLogin(1))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Accounts.Where(i => i.RoleID == 4).ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult ConfirmLandlord(int id)
        {
            var item = _context.Accounts.Find(id);
            if (item != null)
            {
                item.RoleID = 2;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
