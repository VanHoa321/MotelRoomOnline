using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Components
{
    [ViewComponent(Name = "RentalMonth")]
    public class RentalMonthComponent : ViewComponent
    {
        private readonly DataContext _context;
        public RentalMonthComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rList = (from r in _context.RentalMonths
                          where (r.ContractId == Functions.ContractId)
                          orderby r.StartDate descending
                          select r).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", rList));
        }
    }
}
