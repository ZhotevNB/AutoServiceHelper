using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class ShopService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OfferId { get; set; }

        [ForeignKey(nameof(OfferId))]
        public Offer Offer { get; set; }

        [Required]
        [StringLength(DataConstants.NameLength)]
        public string Name { get; set; }

        public TypeActivity Type { get; set; }

        public IList<Part> Parts { get; set; } = new List<Part>();

        public double NeededHourOfWork { get; set; }

        public decimal PricePerHouer { get; set; }

        public decimal Price
        {
            get { return Price; }
            set { Price = Parts.Select(x => x.Price).Sum()+(decimal)NeededHourOfWork*PricePerHouer;}
        }
    }
}
