using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Server.Inventory.Models
{
    //[Serializable]
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Index]
        public bool IsActive { get; set; }

        [Index("UniquePhone", IsUnique = true)]
        [MaxLength(20)]
        public override string PhoneNumber { get; set; }

        [MaxLength(128)]
        [Index("IX_ShopId")]
        public string ShopId { get; set; }

        [Index("IX_RoleName")]
        [MaxLength(128)]
        public string RoleName { get; set; }

        [NotMapped]
        public string ConnectionId { get; set; }
    }
}