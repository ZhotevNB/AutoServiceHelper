using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class AutoShop
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(DataConstants.NameLength)]
        public string Name { get; set; }

        [Required]
        public string ManegerId { get; set; }
        
        public decimal PricePerHour { get; set; }

        [Required]
        public int ContactInfoId { get; set; }


        [ForeignKey(nameof(ContactInfoId))]
        public ContactInfo ContactInfo { get; set; }

        
        public IList<AutoShopActivity> Activities { get; set; } = new List<AutoShopActivity>();

        public IList<Mechanic> Mechanics { get; set; } = new List<Mechanic>();

        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
