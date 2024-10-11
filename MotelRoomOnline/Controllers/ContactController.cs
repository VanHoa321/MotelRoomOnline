using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Controllers
{
    public class ContactController : Controller
    {
        private readonly DataContext _context;
        public ContactController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(int AccountId, string Message)
        {
            Contact create = new Contact();
            create.AccountId = AccountId;
            create.Message = Message;
            create.IsRead = false;
            _context.Contacts.Add(create);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
