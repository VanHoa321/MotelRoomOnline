using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using MotelRoomOnline.Utilities;
using MotelRoomOnline.Models;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class ExportExcelController : Controller
    {
        private readonly DataContext _context;

        public ExportExcelController(DataContext context)
        {
            _context = context;
        }
        public IActionResult ExportInvoiceRoom(int roomId)
        {
            var items = (from invoice in _context.Invoices
                            join rentalMonth in _context.RentalMonths on invoice.RentalMonthId equals rentalMonth.RentalMonthId
                            join contract in _context.Contracts on rentalMonth.ContractId equals contract.ContractId
                            join room in _context.Rooms on contract.RoomId equals room.RoomId
                            join customer in _context.Customers on contract.CustomerId equals customer.CustomerId
                            where room.RoomId == roomId
                            select new
                            {
                                Room = room,
                                Invoice = invoice,
                                RentalMonth = rentalMonth,
                                Contract = contract,
                                Customer = customer
                            }).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("LichSuThuePhong");
                worksheet.Cell(2, 5).Value = "Lịch sử thuê của trọ: " + (items.FirstOrDefault()?.Room.RoomName ?? "Chưa cập nhật");
                worksheet.Cell(2, 5).Style.Font.Bold = true;
                worksheet.Cell(2, 5).Style.Font.FontSize = 16;

                worksheet.Cell(4, 3).Value = "STT";
                worksheet.Cell(4, 4).Value = "Tên Khách Hàng";
                worksheet.Cell(4, 5).Value = "Mã CCCD";
                worksheet.Cell(4, 6).Value = "Số Điện Thoại";
                worksheet.Cell(4, 7).Value = "Ngày sinh";
                worksheet.Cell(4, 8).Value = "Thời gian ở";
                worksheet.Cell(4, 9).Value = "Tổng tiền";

                worksheet.Range("C4:I4").Style.Font.Bold = true;

                int row = 5;
                int index = 1;
                foreach (var item in items)
                {
                    worksheet.Cell(row, 3).Value = index++;
                    worksheet.Cell(row, 4).Value = item.Customer.FullName;
                    worksheet.Cell(row, 5).Value = item.Customer.Code;
                    worksheet.Cell(row, 6).Value = item.Customer.Phone;
                    worksheet.Cell(row, 7).Value = item.Customer.DOB.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 8).Value = "Từ " + item.RentalMonth.StartDate.ToString("dd/MM/yyyy") + " đến " + item.RentalMonth.EndDate.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 9).Value = item.Invoice.TotalAmount;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "LichSuThuePhong.xlsx");
                }
            }
        }

        public IActionResult ExportInvoiceCus(int cusId)
        {
            var items = (from invoice in _context.Invoices
                            join rentalMonth in _context.RentalMonths on invoice.RentalMonthId equals rentalMonth.RentalMonthId
                            join contract in _context.Contracts on rentalMonth.ContractId equals contract.ContractId
                            join room in _context.Rooms on contract.RoomId equals room.RoomId
                            join customer in _context.Customers on contract.CustomerId equals customer.CustomerId
                            where customer.CustomerId == cusId
                            select new
                            {
                                Invoice = invoice,
                                RentalMonth = rentalMonth,
                                Contract = contract,
                                Room = room,
                                Cus = customer
                            }).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("LichSuThuePhong");
                worksheet.Cell(2, 5).Value = "Lịch sử thuê của khách hàng: " + (items.FirstOrDefault()?.Cus.FullName ?? "Chưa cập nhật");
                worksheet.Cell(2, 5).Style.Font.Bold = true;
                worksheet.Cell(2, 5).Style.Font.FontSize = 16;

                worksheet.Cell(4, 3).Value = "STT";
                worksheet.Cell(4, 4).Value = "CCCD";
                worksheet.Cell(4, 5).Value = "Tên Trọ";
                worksheet.Cell(4, 6).Value = "Địa Chỉ";
                worksheet.Cell(4, 7).Value = "Giá phòng";
                worksheet.Cell(4, 8).Value = "Thời gian ở";
                worksheet.Cell(4, 9).Value = "Tổng tiền";

                worksheet.Range("C4:I4").Style.Font.Bold = true;

                int row = 5;
                int index = 1;
                foreach (var item in items)
                {
                    worksheet.Cell(row, 3).Value = index++;
                    worksheet.Cell(row, 4).Value = item.Cus.Code;
                    worksheet.Cell(row, 5).Value = item.Room.RoomName;
                    worksheet.Cell(row, 6).Value = item.Room.Address;
                    worksheet.Cell(row, 7).Value = item.RentalMonth.PriceRoom;
                    worksheet.Cell(row, 8).Value = "Từ " + item.RentalMonth.StartDate.ToString("dd/MM/yyyy") + " đến " + item.RentalMonth.EndDate.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 9).Value = item.Invoice.TotalAmount;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "LichSuThuePhong.xlsx");
                }
            }
        }
    }
}
