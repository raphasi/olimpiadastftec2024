namespace CRM.WebApp.Ingresso.Models
{
    public class UserInfoViewModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public IList<string>? roles { get; set; }
        public Guid? leadID { get; set; }
        public Guid? userID { get; set; }
        public Guid? objectID { get; set; }
        public string? securityIdentifier { get; set; }
    }
}
