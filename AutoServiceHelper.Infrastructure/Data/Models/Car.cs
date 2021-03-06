using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Car
    {
        
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [StringLength(DataConstants.NameLength)]
        public string Manifacture { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser CarOwner { get; set; }

        [StringLength(DataConstants.NameLength)]
        public string Model { get; set; }

        [Range(1900, 2022)]
        public int Year { get; set; }

        [Required]
        [StringLength(DataConstants.VinLength)]
        public string Vin { get; set; }

     
        [Required]
        [StringLength(DataConstants.ColorLength)]
        public string Color { get; set; }

        public IList<Issue> Issues { get; set; }= new List<Issue>();
    }
}