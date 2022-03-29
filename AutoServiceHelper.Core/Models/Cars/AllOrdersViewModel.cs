
using AutoServiceHelper.Infrastructure.Data.Constants;

namespace AutoServiceHelper.Core.Models.Cars
{
    public class AllOrdersViewModel
    {
        
        public Guid Id { get; set; }
             
        public Guid IssueId { get; set; }

        public Guid OfferId { get; set; }
       
        public string Offer { get; set; }

        public string MechanicId { get; set; }

        public OrderStatus Status { get; set; }
               
        public int? CarOdometer { get; set; }
    }
}
