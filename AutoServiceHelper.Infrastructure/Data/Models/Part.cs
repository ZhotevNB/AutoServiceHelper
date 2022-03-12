using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Part
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();

        [Required]
        [StringLength(DataConstants.PartNumberLength)]
        public string Number { get; set; }

        [Required]
        [StringLength(DataConstants.NameLength)]
        public string Name { get; set; }

        public int QuantitiNeeded { get; set; }

        public decimal Price { get; set; }

    }
}
