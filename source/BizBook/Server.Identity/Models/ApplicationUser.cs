using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Server.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            Claim c = new Claim("ShopId", this.ShopId, ClaimValueTypes.String);
            userIdentity.AddClaim(c);
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
    }
}