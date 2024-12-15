using ClosedXML.Excel;
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

        public IActionResult ExportCustomerList()
        {
            var items = _context.Customers.Where(c => c.AccountId == Functions.account.AccountId).OrderByDescending(c => c.CustomerId).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachKhachHang");

                worksheet.Cell(4, 3).Value = "STT";
                worksheet.Cell(4, 4).Value = "Mã Khách Hàng";
                worksheet.Cell(4, 5).Value = "Mã CCCD";
                worksheet.Cell(4, 6).Value = "Tên Khách Hàng";
                worksheet.Cell(4, 7).Value = "Ngày sinh";
                worksheet.Cell(4, 8).Value = "Số Điện Thoại";
                worksheet.Cell(4, 9).Value = "Giới tính";

                worksheet.Range("C4:I4").Style.Font.Bold = true;

                int row = 5;
                int index = 1;
                foreach (var item in items)
                {
                    worksheet.Cell(row, 3).Value = index++;
                    worksheet.Cell(row, 4).Value = item.CustomerId;
                    worksheet.Cell(row, 5).Value = item.Code;
                    worksheet.Cell(row, 6).Value = item.FullName;
                    worksheet.Cell(row, 7).Value = item.DOB.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 8).Value = item.Phone;
                    worksheet.Cell(row, 9).Value = (item.Gender ?? false) ? "Nam" : "Nữ";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DanhSachKhachHang.xlsx");
                }
            }
        }
    }
}
