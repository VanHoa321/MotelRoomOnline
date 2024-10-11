using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "RoomNew")]
    public class RoomNewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public RoomNewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from r in _context.Rooms
                        where (r.RoomStatusId == 1)
                        orderby r.RoomId descending
                        select r).Take(4).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
