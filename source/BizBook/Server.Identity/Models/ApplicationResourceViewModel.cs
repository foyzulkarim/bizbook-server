namespace Server.Identity.Models
{
    public class ApplicationResourceViewModel
    {
        public string ApplicationResourceId { get; set; }
        public string ApplicationResourceName { get; set; }

        public ApplicationResourceViewModel(string applicationResourceId, string applicationResourceName)
        {
            ApplicationResourceId = applicationResourceId;
            ApplicationResourceName = applicationResourceName;
        }
    }
}