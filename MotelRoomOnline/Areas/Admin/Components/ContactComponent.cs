using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;

namespace MotelRoomOnline.Areas.Admin.Components
{
    [ViewComponent(Name = "Contact")]
    public class ContactComponent : ViewComponent
    {
        private readonly DataContext _context;
        public ContactComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mnList = (from mn in _context.Contacts
                          where (mn.IsRead == false)
                          orderby mn.ContactId descending
                          select mn).Take(3).ToList();
            ViewBag.count = _context.Contacts.Where(m => (m.IsRead == false)).ToList();
            ViewBag.Account = _context.Accounts.Where(m => (m.Status == true)).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", mnList));
        }
    }
}
