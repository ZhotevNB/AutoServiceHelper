using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class MechanicActivity
    {
        [Range(0, 100)]
        public int ActivityId { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; }


        public string UserId { get; set; }

        [ForeignKey(nameof (UserId))]
        public Mechanic Mechanic { get; set; }
    }
}
