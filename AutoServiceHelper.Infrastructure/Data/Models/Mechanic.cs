using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Mechanic
    {
       [Key]
       public string UserId { get; set; }
              

        public Guid AutoShopId { get; set; }

        public IList<MechanicActivity> Activities { get; set; } = new List<MechanicActivity>();
    }
}
