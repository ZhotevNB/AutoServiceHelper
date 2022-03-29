using AutoServiceHelper.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceHelper.Infrastructure.Data.Models
{
    public class UserInfo 
    {
        [Key]
        public Guid Id { get; set; }=Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        [StringLength(DataConstants.NameLength)]
        public string NickName { get; set; }

       
        [StringLength(DataConstants.NameLength)]
        public string FirstName { get; set ; }

        
        [StringLength(DataConstants.NameLength)]
        public string LastName { get; set; }

        public ContactInfo? ContactInfo { get; set; }

        public bool AskToChangeRollMechanic { get; set; } = false;

        public bool AskToChangeRollManager { get; set; } = false;
    }
}
