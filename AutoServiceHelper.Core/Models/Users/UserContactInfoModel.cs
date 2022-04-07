

using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Core.Models.Users
{
    public class UserContactInfoModel
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
        [RegularExpression(@"^([+0-9]*)$", ErrorMessage = "The phone number must contain only digits")]
        public string PhoneNumber { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
