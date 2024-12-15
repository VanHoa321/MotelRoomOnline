using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly DataContext _context;

        public ContactController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var items = _context.Contacts.OrderByDescending(i => i.ContactId).ToList();
            ViewBag.Account = _context.Accounts.Where(i => (i.RoleID == 3) || (i.RoleID == 4)).ToList();
            return View(items);
        }

        public IActionResult Details(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewBag.Account = _context.Accounts.FirstOrDefault(i => i.AccountId == contact.AccountId);
            contact.IsRead = true;
            _context.SaveChanges();
            return View(contact);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.Contacts.Find(id);
            if (item != null)
            {
                _context.Contacts.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
