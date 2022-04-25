using AutoServiceHelper.Core.Models.Offers;

namespace AutoServiceHelper.Core.Models.Orders
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public Guid IssueId { get; set; }

        public IssueOrderViewModel Issue { get; set; }

        public OfferViewModel Offer { get; set; }
         
        public string Status { get; set; }

        public int? CarOdometer { get; set; }
    }
}
