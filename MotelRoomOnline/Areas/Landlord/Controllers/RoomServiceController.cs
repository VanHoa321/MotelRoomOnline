using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class RoomServiceController : Controller
    {
        private readonly DataContext _context;

        public RoomServiceController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(long id)
        {
            var roomData = await (from r in _context.Rooms
                                  where r.RoomId == id
                                  select r).FirstOrDefaultAsync();

            if (roomData == null)
            {
                return NotFound("Không tìm thấy phòng.");
            }

            // Lấy danh sách các dịch vụ hiện tại cho phòng cùng với giá
            var currentServices = await _context.RoomServices
            .Where(rc => (rc.RoomId == roomData.RoomId && rc.IsActive == true))
            .Select(rc => new { rc.ServiceId, rc.Price })
            .ToListAsync();

            var allServices = await _context.Services.Where(c => c.IsActive == true).ToListAsync();

            var viewModel = new RoomServiceViewModel
            {
                Room = roomData,
                AllServices = allServices,
                SelectedServicesIds = currentServices.Select(cs => cs.ServiceId).ToList(),
                ServicePrices = currentServices.ToDictionary(cs => cs.ServiceId, cs => cs.Price)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveServices(RoomServiceViewModel viewModel)
        {
            if (viewModel?.Room == null || viewModel.SelectedServicesIds == null || viewModel.ServicePrices == null)
            {
                Functions.message = "Chưa điền giá tiền cho dịch vụ";
                return RedirectToAction("Index", new { id = viewModel?.Room?.RoomId });
            }

            var roomData = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == viewModel.Room.RoomId);

            if (roomData == null)
            {
                return NotFound("Không tìm thấy phòng.");
            }

            // Lấy các dịch vụ hiện tại đã gắn với phòng
            var currentServices = await _context.RoomServices
                .Where(rc => rc.RoomId == roomData.RoomId)
                .ToListAsync();

            // Cập nhật các dịch vụ không còn được chọn (IsActive = false)
            foreach (var roomService in currentServices)
            {
                if (!viewModel.SelectedServicesIds.Contains(roomService.ServiceId))
                {
                    roomService.IsActive = false;
                }
            }

            // Thêm hoặc cập nhật các dịch vụ đã chọn
            foreach (var serviceId in viewModel.SelectedServicesIds)
            {
                var roomService = currentServices.FirstOrDefault(rs => rs.ServiceId == serviceId);

                if (roomService == null)
                {
                    // Nếu dịch vụ chưa tồn tại, thêm mới
                    roomService = new RoomService
                    {
                        RoomId = roomData.RoomId,
                        ServiceId = serviceId,
                        Price = viewModel.ServicePrices.ContainsKey(serviceId) ? viewModel.ServicePrices[serviceId] : 0,
                        IsActive = true
                    };
                    await _context.RoomServices.AddAsync(roomService);
                }
                else
                {
                    // Nếu dịch vụ đã tồn tại, cập nhật giá
                    roomService.Price = viewModel.ServicePrices.ContainsKey(serviceId) ? viewModel.ServicePrices[serviceId] : roomService.Price;
                    roomService.IsActive = true;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = roomData.RoomId });
        }

    }
}
