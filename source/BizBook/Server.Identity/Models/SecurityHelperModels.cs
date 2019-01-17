namespace Server.Identity.Models
{
    public class PermissionRequest
    {
        public string Name { get; set; }
    }

    public class ResourcePermissionViewModel
    {
        public string Id { get; set; }
        public string ApplicationResourceId { get; set; }
        public string RoleId { get; set; }
        public bool IsAllowed { get; set; }
        public string Name { get; set; }
    }
}
