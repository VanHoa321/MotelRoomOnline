using Microsoft.AspNetCore.Mvc;
using MotelRoomOnline.Models;
using MotelRoomOnline.Models.ViewModels;
using MotelRoomOnline.Utilities;

namespace MotelRoomOnline.Controllers
{
    public class PostController : Controller
    {
        private readonly DataContext _context;
        public int pageSize = 2;
        public PostController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var post = _context.Posts.Where(p => p.IsActive == true).OrderByDescending(p =>p.PostId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Account = _context.Accounts.ToList();
            var postListViewModel = new PostListViewModel
            {
                Posts = post,
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = _context.Posts.Where(p => p.IsActive == true).Count()
                }
            };
            return View(postListViewModel);
        }

        [Route("/bai-viet/{alias}-{id}.html")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = _context.Posts.FirstOrDefault(m => (m.PostId == id) && (m.IsActive == true));

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.postComment = _context.PostComments.Where(m => (m.IsActive == true) && (m.PostId == id)).OrderByDescending(cm => cm.PostCommentId).ToList();
            ViewBag.Account = _context.Accounts.Where(m => (m.Status == true)).ToList();
            return View(post);
        }

        [HttpPost]
        public IActionResult Create(int AccountId, string Message, int PostId)
        {
            PostComment create = new PostComment();
            create.AccountId = AccountId;
            create.PostId = PostId;
            create.Content = Message;
            create.CreatedDate = DateTime.Now ;
            create.IsActive = true ;
            _context.PostComments.Add(create);
            _context.SaveChanges();
            var cmt = _context.PostComments.Where(m => (m.IsActive == true) && (m.PostId == PostId)).OrderByDescending(cm => cm.PostCommentId).FirstOrDefault();
            var acc = _context.Accounts.FirstOrDefault(a => AccountId == cmt.AccountId);
            return Json(new { success = true, comment = cmt, account = acc});
        }
    }
}
