using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "Slider")]
    public class SliderComponent : ViewComponent
    {
        private readonly DataContext _context;
        public SliderComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from p in _context.Sliders
                        where (p.IsActive == true)
                        orderby p.Position ascending
                        select p).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
