using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Components
{
    [ViewComponent(Name = "ContactView")]
    public class ContactViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ContactViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from i in _context.Contacts
                        orderby i.ContactId descending
                        select i).Take(4).ToList();
            ViewBag.Account = _context.Accounts.Where(i => i.Status == true).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
    }
}
