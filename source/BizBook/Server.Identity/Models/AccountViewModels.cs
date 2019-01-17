using System.Collections.Generic;

namespace Server.Identity.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
    public class AppUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public string ShopId { get; set; }
    }
    public class AppUserRolesViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }

    }

    public class AppResourceViewModel
    {
        public AppResourceViewModel(ApplicationResource x)
        {
            Id = x.Id;
            Name = x.Name;
            IsPublic = x.IsPublic;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }

    public class AppPermissionViewModel
    {
        public AppPermissionViewModel(ApplicationPermission x)
        {
            Id = x.Id;
            ResourceId = x.ResourceId;
            RoleId = x.RoleId;
            IsAllowed = x.IsAllowed;
            Resource = x.Resource.Name;
            Role = x.Role.Name;
        }
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string RoleId { get; set; }
        public bool IsAllowed { get; set; }
        public string Resource { get; set; }
        public string Role { get; set; }
    }

    public class AppRoleViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string DefaultRoute { get; set; }
    }

}
