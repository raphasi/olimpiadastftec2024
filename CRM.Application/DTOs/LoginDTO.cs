using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Password { get; set; }
        public Guid? LeadID { get; set; }
        public Guid? UserID { get; set; }
        public Guid? ObjectID { get; set; }
        public string? UserName { get; set; }
        public string? SecurityIdentifier { get; set; }
    }
}
