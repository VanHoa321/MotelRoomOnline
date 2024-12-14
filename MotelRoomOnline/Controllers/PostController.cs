using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [Route("bai-viet")]
        public IActionResult Index()
        {      
            ViewBag.postC = _context.PostCategories.ToList();
            return View();
        }

        public IActionResult GetData(int page = 1, string searchKey = "", int categoryId = 0)
        {
            var query = from p in _context.Posts
                        join a in _context.Accounts on p.AccountId equals a.AccountId
                        where p.IsActive == true
                        select new
                        {
                            Post = p,
                            PremiumId = a.PremiumId
                        };

            if (!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(p => p.Post.PostTitle.Contains(searchKey) || p.Post.Abstract.Contains(searchKey));
            }

            if (categoryId > 0)
            {
                query = query.Where(p => p.Post.PostCategoryId == categoryId);
            }

            // Sắp xếp theo PremiumId giảm dần, sau đó theo PostId giảm dần
            var sortedQuery = query
                .OrderByDescending(p => p.PremiumId)
                .ThenByDescending(p => p.Post.PostId);

            // Lấy bài viết phân trang
            var posts = sortedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => p.Post)
                .ToList();

            // Lấy tài khoản liên quan
            var accountIds = posts.Select(p => p.AccountId).Distinct().ToList();
            var accounts = _context.Accounts
                .Where(a => accountIds.Contains(a.AccountId))
                .ToList();

            // Lấy bình luận liên quan
            var postIds = posts.Select(p => p.PostId).Distinct().ToList();
            var comments = _context.PostComments
                .Where(c => postIds.Contains(c.PostId))
                .ToList();

            var postListViewModel = new PostListViewModel
            {
                Posts = posts,
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = query.Count()
                }
            };
            return Json(new { data = postListViewModel, acc = accounts, cmt = comments });
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
