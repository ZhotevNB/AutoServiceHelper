using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid IssueId { get; set; }

        [ForeignKey(nameof(IssueId))]
        public Issue Issue { get; set; }

        [Required]
        public Guid OfferId { get; set; }

        [ForeignKey(nameof(OfferId))]
        public Offer Offer { get; set; }

        public string? MechanicId { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Waiting;

      
        [Range(DataConstants.OddometerMinValue, DataConstants.OddometerMaxValue)]
        public int? CarOdometer { get; set; }
    }
}
