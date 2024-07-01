using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Ingresso.Models
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
        [Required(ErrorMessage = "Nome de usuário é obrigatório")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Nome Completo é obrigatório")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string? PhoneNumber { get; set; }
    }
}