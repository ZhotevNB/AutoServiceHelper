using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class AutoShopActivity
    {

        [Range(0, 100)]
        public int ActivityId { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; }

        public Guid AutoShopId { get; set; }

        [ForeignKey(nameof(AutoShopId))]
        public AutoShop AutoShop { get; set; }
    }
}
