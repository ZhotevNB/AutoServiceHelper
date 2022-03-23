using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;



namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Mechanic
    {
       [Key]
       [Required]
       public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        public Guid AutoShopId { get; set; }

        public IList<MechanicActivity> Activities { get; set; } = new List<MechanicActivity>();
    }
}
