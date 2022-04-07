using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Core.Models.AutoShop
{
    public class ShopContactInfoModel
    {
        public int? Id { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string AdditionalInfo { get; set; }
    }

}
