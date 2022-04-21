using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Models.Offers
{
    public class ServiceViewModel
    {

        public string Name { get; set; }
        public Guid ServiceId { get; set; }

        public Guid offerId { get; set; }

        public TypeActivity Type { get; set; }
      
        public double NeededHourOfWork { get; set; }

        public decimal PricePerHouer { get; set; }

        public decimal Price { get; set; }

    }
}
