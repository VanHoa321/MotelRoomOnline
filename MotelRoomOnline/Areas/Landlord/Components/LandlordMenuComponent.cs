using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Areas.Landlord.Components
{
    [ViewComponent(Name = "LandlordMenu")]
    public class LandlordMenuComponent : ViewComponent
    {
        private readonly DataContext _context;
        public LandlordMenuComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mnList = (from mn in _context.AdminMenus
                          where (mn.IsActive == true && mn.AreaName == ("Landlord"))
                          select mn).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", mnList));
        }
    }
}
