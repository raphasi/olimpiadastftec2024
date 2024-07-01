using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Ingresso.Models
{
    public class CartHeaderDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        public string NameOnCard { get; set; }
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        public string CVV { get; set; }
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        public string ExpireMonthYear { get; set; }
        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        public string CouponCode { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }

        [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "O Nome Completo deve ter entre 2 e 200 caracteres.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O campo Primeiro Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O Primeiro Nome deve ter entre 2 e 100 caracteres.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O Sobrenome deve ter entre 2 e 100 caracteres.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email deve ser válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [Phone(ErrorMessage = "O Telefone deve ser válido.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "O campo Rua é obrigatório.")]
        [StringLength(200, ErrorMessage = "O Endereço 1 não pode exceder 200 caracteres.")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [StringLength(20, ErrorMessage = "O Código Postal não pode exceder 20 caracteres.")]
        public string Address_PostalCode { get; set; }

        [Required(ErrorMessage = "O campo País é obrigatório.")]
        [StringLength(100, ErrorMessage = "O País não pode exceder 100 caracteres.")]
        public string Address_Country { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Estado não pode exceder 100 caracteres.")]
        public string Address_State { get; set; }

        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [StringLength(100, ErrorMessage = "A Cidade não pode exceder 100 caracteres.")]
        public string Address_City { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [StringLength(200, ErrorMessage = "O Complemento não pode exceder 200 caracteres.")]
        public string Address_Adjunct { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Cliente é obrigatório.")]
        [Range(0, 9999, ErrorMessage = "O Tipo de Cliente deve ser um valor válido.")]
        public int TypeLead { get; set; }

        public string CPF { get; set; }

        public string CNPJ { get; set; }
    }
}