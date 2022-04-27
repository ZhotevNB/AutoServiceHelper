using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       public DbSet<AutoShop> AutoShops { get; set; }
       public DbSet<ShopService> ShopServices { get; set; }
       public DbSet<Mechanic> Mechanics { get; set; }
       public DbSet<Offer> Offers { get; set; }
       public DbSet<Order> Orders { get; set; }     
       public DbSet<Issue> Issues { get; set; }
       public DbSet<Part> Parts { get; set; }
       public DbSet<Car> Cars { get; set; }
       public DbSet<Activity> Activities { get; set; }
       public DbSet<ContactInfo> ContactsInfo { get; set; }
       public DbSet<ShopManeger> ShopManegers { get; set; }
       public DbSet<UserInfo> UsersInfo { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutoShopActivity>()
                .HasKey(e => new
                {
                    e.ActivityId,
                    e.AutoShopId
                });
            modelBuilder.Entity<MechanicActivity>()
               .HasKey(e => new
               {
                   e.ActivityId,
                   e.MechanicId
               });
         
            base.OnModelCreating(modelBuilder);
        }

    }
}