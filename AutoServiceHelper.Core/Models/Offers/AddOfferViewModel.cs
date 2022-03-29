namespace AutoServiceHelper.Core.Models.Offers
{
    public class AddOfferViewModel
    {      
        public IList<AddShopServiceViewModel> Services { get; set; } = new List<AddShopServiceViewModel>();

        public string AdditionalInfo { get; set; }

    }
}
