using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Core.Models.Issues
{
    public class AddIssueFormModel
    {
        [Required]
        public TypeActivity Type { get; set; }
                          
        [Required]
        [Range(DataConstants.OddometerMinValue, DataConstants.OddometerMaxValue)]
        public int CarOdometer { get; set; }
          
        [Required]
        [MinLength(DataConstants.DiscriptionMinLen)]
        public string Description { get; set; }

        public IEnumerable<string> ListTypes { get; set; }

    }
}
