using AutoServiceHelper.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class Activity
    {
        [Key]
        [Range(0,100)]
        public int Id { get; set; }

        public TypeActivity ActivityName { get; set; }
    }
}
