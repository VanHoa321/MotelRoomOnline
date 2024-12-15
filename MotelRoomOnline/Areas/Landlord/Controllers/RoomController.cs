using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class RoomController : Controller
    {
        private readonly DataContext _context;

        public RoomController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin(2))
            {
                return Redirect("/Login/Index");
            }
            ViewBag.roomStatus = _context.RoomStatuses.ToList();
            var items = _context.Rooms.Where(r => r.AccountId == Functions.account.AccountId).OrderByDescending(r => r.RoomId).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            var listWard = (from w in _context.Wards
                            select new SelectListItem()
                            {
                                Text = w.WardName,
                                Value = w.WardId.ToString(),
                            }).ToList();
            listWard.Insert(0, new SelectListItem()
            {
                Text = "Chọn Phường/Xã",
                Value = "0"
            });

            var listRoomCategories = (from r in _context.RoomCategories.Where(r => r.IsActive == true)
                                      select new SelectListItem()
                                      {
                                          Text = r.RoomCategoryName,
                                          Value = r.RoomCategoryId.ToString(),
                                      }).ToList();
            listRoomCategories.Insert(0, new SelectListItem()
            {
                Text = "Chọn phân loại",
                Value = "0"
            });

            var listRoomStatus = (from r in _context.RoomStatuses.Where(r => r.IsActive == true)
                                  select new SelectListItem()
                                  {
                                      Text = r.RoomStatusName,
                                      Value = r.RoomStatusId.ToString(),
                                  }).ToList();
            listRoomStatus.Insert(0, new SelectListItem()
            {
                Text = "Chọn trạng thái",
                Value = "0"
            });
            ViewBag.listWard = listWard;
            ViewBag.listRoomCategories = listRoomCategories;
            ViewBag.listRoomStatus = listRoomStatus;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room create)
        {
            if (ModelState.IsValid)
            {
                create.AccountId = Functions.account.AccountId;
                create.Alias = Functions.TitleSlugGenerationAlias(create.RoomName);
                _context.Rooms.Add(create);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(create);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _context.Rooms.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            var listWard = (from w in _context.Wards
                            select new SelectListItem()
                            {
                                Text = w.WardName,
                                Value = w.WardId.ToString(),
                            }).ToList();
            listWard.Insert(0, new SelectListItem()
            {
                Text = "Chọn Phường/Xã",
                Value = "0"
            });

            var listRoomCategories = (from r in _context.RoomCategories.Where(r => r.IsActive == true)
                                      select new SelectListItem()
                                      {
                                          Text = r.RoomCategoryName,
                                          Value = r.RoomCategoryId.ToString(),
                                      }).ToList();
            listRoomCategories.Insert(0, new SelectListItem()
            {
                Text = "Chọn phân loại",
                Value = "0"
            });

            var roomStatus = _context.RoomStatuses.FirstOrDefault(r => r.RoomStatusId == item.RoomStatusId);
            ViewBag.listWard = listWard;
            ViewBag.listRoomCategories = listRoomCategories;
            ViewBag.roomStatus = roomStatus;
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Room edit)
        {
            if (ModelState.IsValid)
            {
                edit.AccountId = Functions.account.AccountId;
                edit.Alias = Functions.TitleSlugGenerationAlias(edit.RoomName);
                _context.Rooms.Update(edit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edit);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var item = _context.Rooms.Find(id);
            var isRoomRented = _context.Contracts.Any(c => c.RoomId == item.RoomId);
            if (isRoomRented)
            {
                return Json(new { success = false });
            }
            if (item != null)
            {
                _context.Rooms.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult GetData()
        {
            var items = _context.Rooms.Where(r => r.AccountId == Functions.account.AccountId).OrderByDescending(r => r.RoomId).ToList();
            var roomS = _context.RoomStatuses.ToList();
            return Json(new { data = items, totalItems = items.Count, roomStatus = roomS });
        }

        public IActionResult ExportMotelRoomList()
        {
            var items = _context.Rooms.Where(r => r.AccountId == Functions.account.AccountId).OrderByDescending(r => r.RoomId).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachPhongTro");

                worksheet.Cell(4, 3).Value = "STT";
                worksheet.Cell(4, 4).Value = "Mã Phòng Trọ";
                worksheet.Cell(4, 5).Value = "Tên phòng trọ";
                worksheet.Cell(4, 6).Value = "Địa chỉ";
                worksheet.Cell(4, 7).Value = "Giá thuê";
                worksheet.Cell(4, 8).Value = "Diện tích";
                worksheet.Cell(4, 9).Value = "Số người ở tối đa";
                worksheet.Cell(4, 10).Value = "Ngày tạo";
                worksheet.Cell(4, 11).Value = "Trạng thái phòng";

                worksheet.Range("C4:K4").Style.Font.Bold = true;

                int row = 5;
                int index = 1;
                foreach (var item in items)
                {
                    worksheet.Cell(row, 3).Value = index++;
                    worksheet.Cell(row, 4).Value = item.RoomId;
                    worksheet.Cell(row, 5).Value = item.RoomName;
                    worksheet.Cell(row, 6).Value = item.Address;
                    worksheet.Cell(row, 7).Value = item.PriceRoom;
                    worksheet.Cell(row, 8).Value = item.Acreage;
                    worksheet.Cell(row, 9).Value = item.MaxPeople;
                    worksheet.Cell(row, 10).Value = item.CreatedDate.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 11).Value = item.RoomStatusId == 1 ? "Còn trống" :
                                                    item.RoomStatusId == 3 ? "Đang cho thuê" :
                                                    item.RoomStatusId == 4 ? "Đang sửa chữa" :
                                                    item.RoomStatusId == 5 ? "Chờ duyệt" : "Null";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DanhSachPhongTro.xlsx");
                }
            }
        }
    }
}
