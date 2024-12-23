using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;
using MotelRoomOnline.Services;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class SendCustomerInvoiceController : Controller
    {
        private readonly DataContext _context;
        private readonly EmailService _emailService;
        public SendCustomerInvoiceController(EmailService emailService, DataContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(long rentalMonthId, decimal totalAmount, List<ServiceEmailModel> services)
        {
            if (services == null || !services.Any())
            {
                return BadRequest("Danh sách dịch vụ không được để trống.");
            }
            var rentalMonth = _context.RentalMonths.Find(rentalMonthId);
            var contract = _context.Contracts.FirstOrDefault(i => i.ContractId == rentalMonth.ContractId);
            var room = _context.Rooms.FirstOrDefault(i => i.RoomId == contract.RoomId);
            var landlord = _context.Accounts.FirstOrDefault(i => i.AccountId == room.AccountId);
            var customer = _context.Customers.FirstOrDefault(i => i.CustomerId == contract.CustomerId);

            var toEmail = "vanhoa12092003@gmail.com";
            var subject = $"Hóa đơn tháng thuê từ {rentalMonth.StartDate.ToString("dd/MM/yyyy")} đến {rentalMonth.EndDate.ToString("dd/MM/yyyy")}";

            var body = "<h1>Thông tin Hóa đơn thanh toán tháng thuê</h1>";
            body += $"<p>Mã tháng thuê: {rentalMonthId}</p>";
            body += $"<p style='color: red; font-weight: bold;'>Tiền cần thanh toán: {Functions.ToVnd(totalAmount)} <sup>đ</sup></p>";
            body += $"<p>Tiền thuê phòng: {Functions.ToVnd(rentalMonth.PriceRoom)} <sup>đ</sup></p>";
            body += $"<p>Chủ trọ: {landlord.FullName}</p>";
            body += $"<p>Số điện thoại chủ trọ: {landlord.Phone}</p>";
            body += $"<p>Emai chủ trọ: {landlord.Email}</p>";
            // Thông tin hợp đồng
            body += "<h2>Thông tin hợp đồng</h2>";
            body += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
            body += "<tr>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Mã hợp đồng</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Ngày tạo</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Tiền cọc</th>" +
                    "</tr>";
            body += $"<tr>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{contract.ContractId}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{contract.CreatedDate.ToString("dd/MM/yyyy")}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{Functions.ToVnd(contract.DepositAmount)} <sup>đ<sup></td>" +
                    $"</tr>";
            body += "</table>";

            // Thông tin phòng thuê
            body += "<h2>Thông tin phòng thuê</h2>";
            body += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
            body += "<tr>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Tên trọ</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Địa chỉ</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Diện tích</th>" +
                    "</tr>";
            body += $"<tr>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{room.RoomName}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{room.Address}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{room.Acreage} m<sup>2</sup></td>" +
                    $"</tr>";
            body += "</table>";

            // Thông tin khách hàng
            body += "<h2>Thông tin khách hàng</h2>";
            body += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
            body += "<tr>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Tên khách hàng</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>CCCD</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>SĐT</th>" +
                        "<th style='border: 1px solid black; padding: 8px;'>Ngày sinh</th>" +
                    "</tr>";
            body += $"<tr>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{customer.FullName}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{customer.Code}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{customer.Phone}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{customer.DOB.Value.ToString("dd/MM/yyyy")}</td>" +
                    $"</tr>";
            body += "</table>";

            // Thông tin dịch vụ sử dụng
            body += "<h2>Danh sách dịch vụ sử dụng</h2>";
            body += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
            body += "<tr><th style='border: 1px solid black; padding: 8px;'>Tên dịch vụ</th>" +
                    "<th style='border: 1px solid black; padding: 8px;'>Số lượng</th>" +
                    "<th style='border: 1px solid black; padding: 8px;'>Giá</th>" +
                    "<th style='border: 1px solid black; padding: 8px;'>Thành tiền</th></tr>";

            foreach (var service in services)
            {
                var totalPrice = service.Price * service.Quantity; // Tính tổng tiền cho từng dịch vụ
                body += $"<tr><td style='border: 1px solid black; padding: 8px;'>{service.ServiceName}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{service.Quantity}</td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{Functions.ToVnd(service.Price)} <sup>đ</sup></td>" +
                        $"<td style='border: 1px solid black; padding: 8px;'>{Functions.ToVnd(totalPrice)} <sup>đ</sup></td></tr>";
            }
            body += "</table>";

            try
            {
                await _emailService.SendEmailAsync(toEmail, subject, body);
                return Json(new { success = true, message = "Email đã được gửi thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Có lỗi xảy ra khi gửi email: {ex.Message}");
            }
        }
    }
}
