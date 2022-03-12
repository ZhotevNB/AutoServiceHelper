using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class CarOwner
    {
        [Key]
        public string UserId { get; set; }

        public IList<Car> Cars { get; set; } = new List<Car>();

    }
}
