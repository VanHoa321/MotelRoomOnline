namespace MotelRoomOnline.Models.ViewModels
{
    public class RoomListViewModel
    {
        public IEnumerable<Room> Rooms { get; set; } = Enumerable.Empty<Room>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
