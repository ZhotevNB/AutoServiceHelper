namespace AutoServiceHelper.Core.Models.Offers
{
    public class OfferViewModel
    {
       
        public Guid IssueId { get; set; }

        public string ShopId { get; set; }

        public DateTime SubmitionDate { get; set; }

        public IList<ServiceViewModel> Services { get; set; } = new List<ServiceViewModel>();

        public string AdditionalInfo { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
