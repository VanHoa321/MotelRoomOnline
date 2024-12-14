namespace MotelRoomOnline.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string Image { get; set; }
        public string OwnerName { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
