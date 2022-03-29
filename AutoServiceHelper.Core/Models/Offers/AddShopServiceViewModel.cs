using AutoServiceHelper.Infrastructure.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceHelper.Core.Models.Offers
{
    public class AddShopServiceViewModel
    {
        public string Name { get; set; }

        public TypeActivity Type { get; set; }

        public IList<AddPartViewModel> Parts { get; set; } = new List<AddPartViewModel>();

        public double NeededHourOfWork { get; set; }

        public decimal PricePerHouer { get; set; }
    }
}
