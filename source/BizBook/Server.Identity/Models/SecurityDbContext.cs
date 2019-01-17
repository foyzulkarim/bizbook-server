using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Server.Identity.Models
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<IdentityUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationPermission> Permissions { get; set; }
        public DbSet<ApplicationResource> Resources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}