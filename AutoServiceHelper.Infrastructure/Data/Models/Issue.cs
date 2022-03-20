using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Issue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public TypeActivity Type { get; set; }

        [Required]
        public string SubmitetByUserId { get; set; }

        public DateTime SubmitionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; }

        [Required] 
        [Range(DataConstants.OddometerMinValue,DataConstants.OddometerMaxValue)]
        public int CarOdometer { get; set; }

        public IssueStatus Status { get; set; } = IssueStatus.WaitingForOffer;

        
        public string Description { get; set; }

        public IList<Offer> Offers { get; set; }= new List<Offer>();


        public string? OfferID { get; set; }
    }
}
