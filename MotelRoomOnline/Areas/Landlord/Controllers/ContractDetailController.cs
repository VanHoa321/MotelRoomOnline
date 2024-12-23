using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Areas.Landlord.Controllers
{
    [Area("Landlord")]
    public class ContractDetailController : Controller
    {
        private readonly DataContext _context;

        public ContractDetailController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(long id)
        {
            var rentalMonth = _context.RentalMonths.FirstOrDefault(rm => rm.RentalMonthId == id);
            if (rentalMonth == null)
            {
                return NotFound();
            }

            var contract = _context.Contracts.FirstOrDefault(c => c.ContractId == Functions.ContractId);
            var contractDetails = _context.ContractDetails.Where(cd => cd.RentalMonthId == id).ToList();
            var roomServices = _context.RoomServices.Where(rs => rs.RoomId == Functions.RoomId).ToList();
            var services = _context.Services.ToList();

            var viewModel = new RentalMonthDetailsViewModel
            {
                RentalMonth = rentalMonth,
                ContractDetails = contractDetails,
                RoomServices = roomServices ?? new List<RoomService>(),
                Services = services,
                ServiceQuantities = new Dictionary<long, int>(),
                ServicePrices = roomServices.ToDictionary(rs => rs.RoomServiceId, rs => rs.Price)
            };
            ViewBag.Customer = _context.Customers.FirstOrDefault(i => i.CustomerId == contract.CustomerId);
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Index(RentalMonthDetailsViewModel model)
        {
            var rentalMonthId = model.RentalMonthId;
            decimal totalAmount = 0;
            var Contract = _context.Contracts.FirstOrDefault(c => c.ContractId == Functions.ContractId);
            decimal priceRoom = Contract.PriceRoom;
            foreach (var serviceId in model.ServiceQuantities.Keys)
            {
                var quantity = model.ServiceQuantities[serviceId];
                if (quantity > 0) 
                {
                    var servicePrice = model.ServicePrices[serviceId];

                    var contractDetail = new ContractDetail
                    {
                        ContractId = Functions.ContractId,
                        RentalMonthId = rentalMonthId,
                        RoomServiceId = serviceId,
                        ServicePrice = servicePrice,
                        ServiceQuantity = quantity,
                        Amount = quantity * servicePrice
                    };
                    totalAmount += contractDetail.Amount;
                    _context.ContractDetails.Add(contractDetail);
                }
            }
            var invoice = new Invoice
            {
                ContractId = Functions.ContractId,
                RentalMonthId = rentalMonthId,
                InvoiceDate = DateTime.Now,
                TotalAmount = totalAmount + priceRoom,
                Status = 1
            };
            _context.Invoices.Add(invoice);
            var rentalMonths = _context.RentalMonths.FirstOrDefault(c => c.RentalMonthId == rentalMonthId);
            if (rentalMonths != null)
            {
                rentalMonths.Status = 2;
                _context.RentalMonths.Update(rentalMonths);
            }
            _context.SaveChanges();
            return Redirect("/Landlord/Contract/Detail/" + Functions.DetailId);
        }

        public IActionResult Detail(int id)
        {
            var contract = _context.Contracts.FirstOrDefault(c => c.ContractId == Functions.ContractId);
            var rentalMonth = _context.RentalMonths.FirstOrDefault(rm => rm.RentalMonthId == id);
            var items = _context.ContractDetails.Where(cd => cd.RentalMonthId == id).ToList();
            var roomServices = _context.RoomServices.Where(rs => rs.RoomId == Functions.RoomId).ToList();
            var invoice = _context.Invoices.FirstOrDefault(i => i.RentalMonthId == id);
            var service = _context.Services.Where(s => s.IsActive == true).ToList();
            var viewModel = new ContractDetaiViewModel
            {
                RentalMonth = rentalMonth,
                Invoice = invoice,
                ContractDetails = items,
                RoomServices = roomServices,
                Services = service

            };
            ViewBag.Customer = _context.Customers.FirstOrDefault(i => i.CustomerId == contract.CustomerId);
            return View(viewModel);
        }
    }
}
