namespace MotelRoomOnline.Models.ViewModels
{
    public class RoomServiceViewModel
    {
        public Room? Room { get; set; }

        public List<Service>? AllServices { get; set; }

        public List<int>? SelectedServicesIds { get; set; }

        public Dictionary<int, decimal>? ServicePrices { get; set; }
    }
}
