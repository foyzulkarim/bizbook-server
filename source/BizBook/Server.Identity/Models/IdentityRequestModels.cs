namespace Server.Identity.Models
{
    public class EmployeeRequestModel 
    {
        public string OrderBy { get; set; }
        public string IsAscending { get; set; }
        public string Id { get; set; }
        public string Keyword { get; set; }
        public string ShopId { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public int Page { get; set; }
    }

}