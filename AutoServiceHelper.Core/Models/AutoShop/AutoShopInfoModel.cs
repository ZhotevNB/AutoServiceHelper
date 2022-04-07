using AutoServiceHelper.Infrastructure.Data.Constants;
using AutoServiceHelper.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Core.Models.AutoShop
{
    public class AutoShopInfoModel
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(DataConstants.NameLength)]
        public string Name { get; set; }

        [Range(0.0,200.0)]
        public decimal PricePerHour { get; set; }
                
        public ShopContactInfoModel ShopContactInfo { get; set; }

        
    }
}
