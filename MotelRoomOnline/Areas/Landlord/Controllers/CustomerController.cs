using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class CustomerController : Controller
    {
        private readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            var items = _context.Customers.Where(c => c.AccountId == Functions.account.AccountId).OrderByDescending(c => c.CustomerId).ToList();
            return View(items);
        }

        public IActionResult Create()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer create)
        {
            if (ModelState.IsValid)
            {
                bool checkIdOrPhone = _context.Customers.Any(c => c.Code == create.Code || c.Phone == create.Phone);
                if (checkIdOrPhone)
                {
                    if (_context.Customers.Any(c => c.Code == create.Code))
                    {
                        Functions.message = "CCCD này đã tồn tại!";
                    }
                    if (_context.Customers.Any(c => c.Phone == create.Phone))
                    {
                        Functions.message = "Số điện thoại này đã tồn tại!";
                    }
                    return View(create);
                }
                Functions.message = string.Empty;
                create.AccountId = Functions.account.AccountId;
                _context.Customers.Add(create);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(create);
        }

        public IActionResult Edit(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _context.Customers.Find(id);
            if (item == null)
            {
                return NotFound();
            }            
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Customer edit)
        {
            if (ModelState.IsValid)
            {                
                bool checkId = _context.Customers.Any(c => c.Code == edit.Code && c.CustomerId != edit.CustomerId);
                if (checkId)
                {
                    Functions.message = "CCCD này đã tồn tại!";
                    return View(edit);
                }
                bool checkPhone = _context.Customers.Any(c => c.Phone == edit.Phone && c.CustomerId != edit.CustomerId);
                if (checkPhone)
                {
                    Functions.message = "Số điện thoại này đã tồn tại!";
                    return View(edit);
                }
                Functions.message = string.Empty;
                _context.Customers.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(long? id)
        {
            var check = _context.Contracts.Find(id);
            if (check != null)
            {
                return Json(new { success = false });
            }
            var item = _context.Customers.Find(id);
            if (item != null)
            {
                _context.Customers.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult GetData()
        {
            var items = _context.Customers.Where(c => c.AccountId == Functions.account.AccountId).OrderByDescending(c => c.CustomerId).Take(10).ToList();
            return Json(new { data = items, totalItems = items.Count});
        }
    }
}
