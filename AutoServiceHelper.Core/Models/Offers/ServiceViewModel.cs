using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Models.Offers
{
    public class ServiceViewModel
    {

        public string Name { get; set; }

        public TypeActivity Type { get; set; }

        public IList<PartsViewModel> Parts { get; set; } = new List<PartsViewModel>();

        public double NeededHourOfWork { get; set; }

        public decimal PricePerHouer { get; set; }

        public decimal Price { get; set; }

    }
}
