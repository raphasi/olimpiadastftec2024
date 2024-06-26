using Newtonsoft.Json.Linq;

namespace CRM.WebApp.Site.Models
{
    public class TokenViewModel
    {
        public string Token { get; }
        public string? RefreshToken { get; }
        public DateTime Expiration { get; }
    }
}
