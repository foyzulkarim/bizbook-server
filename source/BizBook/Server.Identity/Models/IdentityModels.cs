using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Server.Identity.Models
{
    public enum ApplicationRoles
    {
        SuperAdmin = 1,
        ShopAdmin = 2
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name) : base(name)
        {

        }

        public ApplicationRole()
        {

        }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(20)]
        public string DefaultRoute { get; set; }
    }

    public enum ResourceType
    {
        Browser,
        Api,
        Mobile
    }

    [Table("AspNetResources")]
    public class ApplicationResource
    {
        public ApplicationResource()
        {
            Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public ResourceType ResourceType { get; set; }
        public virtual ICollection<ApplicationPermission> Permissions { get; set; }
    }

    [Table("AspNetPermissions")]
    public class ApplicationPermission
    {
        public ApplicationPermission()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string RoleId { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsDisabled { get; set; }

        [ForeignKey("ResourceId")]
        public virtual ApplicationResource Resource { get; set; }
        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
    }
}