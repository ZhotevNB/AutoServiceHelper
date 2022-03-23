using AutoServiceHelper.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
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
        DbSet<AutoShop> AutoShops { get; set; }
        DbSet<ShopService> ShopServices { get; set; }
        DbSet<Mechanic> Mechanics { get; set; }
        DbSet<Offer> Offers { get; set; }
        DbSet<Order> Orders { get; set; }     
        DbSet<Issue> Issues { get; set; }
        DbSet<Part> Parts { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<ContactInfo> ContactsInfo { get; set; }
        DbSet<ShopManeger> ShopManegers { get; set; }
        DbSet<UserInfo> UsersInfo { get; set; }



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
                   e.UserId
               });
         

            modelBuilder.Entity<Mechanic>()
             .HasMany(x => x.Activities)
             .WithOne(x => x.Mechanic)
             .OnDelete(DeleteBehavior.Restrict);

           


            base.OnModelCreating(modelBuilder);
        }

    }
}