namespace MotelRoomOnline.Models.ViewModels
{
    public class ContractDetaiViewModel
    {
        public RentalMonth RentalMonth { get; set; }
        public Invoice Invoice { get; set; }
        public List<ContractDetail> ContractDetails { get; set; }
        public List<RoomService> RoomServices { get; set; }
        public List<Service> Services { get; set; }
    }
}
