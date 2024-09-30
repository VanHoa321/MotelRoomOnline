namespace MotelRoomOnline.Models.ViewModels
{
    public class RentalMonthDetailsViewModel
    {
        public long RentalMonthId { get; set; }
        public RentalMonth RentalMonth { get; set; }
        public List<ContractDetail> ContractDetails { get; set; }
        public List<RoomService> RoomServices { get; set; }
        public List<Service> Services { get; set; }
        public Dictionary<long, int> ServiceQuantities { get; set; }
        public Dictionary<long, decimal> ServicePrices { get; set; }
    }

}
