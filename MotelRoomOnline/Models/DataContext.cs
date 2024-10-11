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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RoomCategories> RoomCategories { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomCriteria> RoomCriterias { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<RentalMonth> RentalMonths { get; set; }
        public DbSet<ContractDetail> ContractDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
    }
}
