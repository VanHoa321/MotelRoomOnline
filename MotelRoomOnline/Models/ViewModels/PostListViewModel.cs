namespace MotelRoomOnline.Models.ViewModels
{
    public class PostListViewModel
    {
        public IEnumerable<Post> Posts { get; set; } = Enumerable.Empty<Post>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
