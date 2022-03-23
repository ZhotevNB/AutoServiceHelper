using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class ShopManeger
    {
        [Required]
        [Key]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        [Required]
        public Guid AutoShopId { get; set; }
        [ForeignKey(nameof(AutoShopId))]
        public AutoShop AutoShop { get; set; }
    }
}
