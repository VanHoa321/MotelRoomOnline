using Microsoft.EntityFrameworkCore;
using MotelRoomOnline.Areas.Admin.Models;

namespace MotelRoomOnline.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AdminMenu> AdminMenus { get; set; }
        public DbSet<PostCategories> PostCategories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
    }
}
