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
            base.OnModelCreating(modelBuilder);
        }
        DbSet<AutoShop> AutoShops { get; set; }
        DbSet<ShopService> ShopServices { get; set; }
        DbSet<Mechanic> Mechanics { get; set; }
        DbSet<Offer> Offers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<CarOwner> CarOwners { get; set; }
        DbSet<Issue> Issues { get; set; }
        DbSet<Part> Parts { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<ContactInfo> ContactsInfo { get; set; }
    }
}