using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "FooterView")]
    public class FooterViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public FooterViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from p in _context.Posts
                        where (p.IsActive == true)
                        orderby p.PostId ascending
                        select p).Take(3).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
