using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class RoomCriteriaController : Controller
    {
        private readonly DataContext _context;

        public RoomCriteriaController(DataContext context)
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

            // Lấy danh sách tiêu chí hiện tại cho phòng
            var currentCriterias = await _context.RoomCriterias
                .Where(rc => rc.RoomId == roomData.RoomId)
                .Select(rc => rc.CriteriaId) // Chỉ lấy CriteriaId
                .ToListAsync();

            var allCriterias = await _context.Criterias.Where(c => c.IsActive == true).ToListAsync();

            var viewModel = new RoomCriteriaViewModel
            {
                Room = roomData,
                AllCriterias = allCriterias,
                SelectedCriteriaIds = currentCriterias // Truyền danh sách các CriteriaId hiện tại
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCriterias(RoomCriteriaViewModel viewModel)
        {
            if (viewModel?.Room == null)
            {
                return NotFound();
            }

            // Lấy thông tin phòng
            var roomData = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == viewModel.Room.RoomId);

            if (roomData == null)
            {
                return NotFound("Không tìm thấy phòng!");
            }

            // Lấy tất cả RoomCriterias liên quan đến phòng
            var currentCriterias = await _context.RoomCriterias
                .Where(rc => rc.RoomId == roomData.RoomId)
                .ToListAsync();

            // Danh sách các CriteriaId hiện tại
            var currentCriteriaIds = currentCriterias.Select(rc => rc.CriteriaId).ToList();

            // Cập nhật các tiêu chí đã có
            foreach (var criteriaId in currentCriteriaIds)
            {
                if (viewModel.SelectedCriteriaIds != null && viewModel.SelectedCriteriaIds.Contains(criteriaId))
                {
                }
                else
                {
                    // Tiêu chí không còn được chọn, xóa nó
                    var criteriaToRemove = currentCriterias.FirstOrDefault(rc => rc.CriteriaId == criteriaId);
                    if (criteriaToRemove != null)
                    {
                        _context.RoomCriterias.Remove(criteriaToRemove);
                    }
                }
            }

            // Thêm các tiêu chí mới
            if (viewModel.SelectedCriteriaIds != null)
            {
                foreach (var criteriaId in viewModel.SelectedCriteriaIds)
                {
                    if (!currentCriteriaIds.Contains(criteriaId))
                    {
                        var roomCriteria = new RoomCriteria
                        {
                            RoomId = roomData.RoomId,
                            CriteriaId = criteriaId,
                            IsActive = true
                        };
                        await _context.RoomCriterias.AddAsync(roomCriteria);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = roomData.RoomId });
        }
    }
}
