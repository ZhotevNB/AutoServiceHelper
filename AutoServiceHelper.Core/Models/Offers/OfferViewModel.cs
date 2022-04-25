namespace AutoServiceHelper.Core.Models.Offers
{
    public class OfferViewModel
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }

        public string ShopId { get; set; }

        public DateTime SubmitionDate { get; set; }       

        public string AdditionalInfo { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }      

        public decimal? TotalPrice { get; set; } 
        public IEnumerable<decimal> PartPrice { get; set; }
        public decimal? ServicePrice { get; set; }

        public bool IsSelected { get; set; }= false;

    }
}
