using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Offer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IssueId { get; set; }

        public string ShopId { get; set; }

        public DateTime SubmitionDate { get; set; } = DateTime.UtcNow;

        public IList<ShopService> Services { get; set; } = new List<ShopService>();

        public string AdditionalInfo { get; set; }

        public decimal TotalPrice { get; set; } = 0;
    }
}
