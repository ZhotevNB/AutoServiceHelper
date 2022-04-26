using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Core.Models.Cars
{
    public class AddCarFormModel
    {
        [Required]
        public string Manifacture { get; init; }

        [Required]
        public string Model { get; init; }

        [Range(1990, 2022)]
        public int Year { get; init; }
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-z])[0-9A-z-]{17}$", ErrorMessage = "Invalid VIN")]
        public string Vin { get; init; }
        [Required]
        [StringLength(30,MinimumLength =3)]
        public string Color { get; init; }

    }
}
