namespace CRM.WebApp.Site.Models
{
    public class DashboardViewModel
    {
        public int CustomerCount { get; set; }
        public int OpportunityCount { get; set; }
        public decimal OpportunityEstimatedValue { get; set; }
        public int QuotesCount { get; set; }
        public int LeadCount { get; set; }
    }
}
