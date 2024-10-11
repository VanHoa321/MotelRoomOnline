using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name ="Menu")]
    public class MenuComponent : ViewComponent
    {
        private readonly DataContext _context;
        public MenuComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from p in _context.Menus
                        where (p.IsActive == true)
                        orderby p.Position ascending
                        select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
