namespace Server.Identity.Models
{
    public class ApplicationPermissionViewModel
    {
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string RoleId { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsDisabled { get; set; }

        public ApplicationPermissionViewModel(string id,string resourceId,string roleId,bool isAllowed)
        {
            Id = id;
            ResourceId = resourceId;
            RoleId = roleId;
            IsAllowed = isAllowed;
        }

        public ApplicationPermissionViewModel(string id, string resourceId, string roleId, bool isAllowed,bool isDisabled)
        {
            Id = id;
            ResourceId = resourceId;
            RoleId = roleId;
            IsAllowed = isAllowed;
            IsDisabled = isDisabled;
        }
    }
}