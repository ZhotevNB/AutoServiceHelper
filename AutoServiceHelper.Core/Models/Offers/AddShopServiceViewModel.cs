using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Core.Models.Offers
{
    public class AddShopServiceViewModel
    {
        [StringLength(30,MinimumLength =4,ErrorMessage ="Length must be between 4 and 30")]
        public string Name { get; set; }

        public string offerId { get; set; }

        public TypeActivity Type { get; set; }

        public IList<AddPartViewModel> Parts { get; set; } = new List<AddPartViewModel>();

        [Range(1,30,ErrorMessage ="Invalid hours")]
        public double NeededHourOfWork { get; set; }

       public decimal PricePerHouer { get; set; }
    }
}
