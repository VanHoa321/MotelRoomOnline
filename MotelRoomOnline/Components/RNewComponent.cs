using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "RNew")]
    public class RNewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public RNewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from r in _context.Rooms
                        where (r.RoomStatusId == 1)
                        orderby r.CreatedDate descending
                        select r).Take(7).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
