using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models
{
    public class RegisterViewModel : EntityBase
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Password { get; set; }
        public Guid? LeadID { get; set; }
        public Guid? UserID { get; set; }
        public Guid? ObjectID { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}