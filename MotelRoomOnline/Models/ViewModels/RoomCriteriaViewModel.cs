namespace MotelRoomOnline.Models.ViewModels
{
    public class RoomCriteriaViewModel
    {
        public Room? Room { get; set; }

        public List<Criteria>? AllCriterias { get; set; }

        public List<int>? SelectedCriteriaIds { get; set; }


    }
}
