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
        DbSet<CarOwner> CarOwners { get; set; }
        DbSet<Issue> Issues { get; set; }
        DbSet<Part> Parts { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<ContactInfo> ContactsInfo { get; set; }
    }
}