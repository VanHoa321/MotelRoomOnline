using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "TopRoomVC")]
    public class TopRoomVCComponent : ViewComponent
    {
        private readonly DataContext _context;
        public TopRoomVCComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from r in _context.Rooms
                        where (r.RoomStatusId == 1)
                        orderby r.ViewCount descending
                        select r).Take(7).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
