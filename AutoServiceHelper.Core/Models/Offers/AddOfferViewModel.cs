using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Core.Models.Offers
{
    public class AddOfferViewModel
    {
        public Guid IssueId { get; set; }

        public string ShopId { get; set; }

        [Required()]
        public string AdditionalInfo { get; set; }

    }
}
