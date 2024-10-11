using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "PostNew")]
    public class PostNewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public PostNewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from p in _context.Posts
                        where (p.IsActive == true)
                        orderby p.CreatedDate descending
                        select p).Take(4).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
