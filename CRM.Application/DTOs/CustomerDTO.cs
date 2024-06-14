using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class CustomerDTO
    {
        public Guid CustomerID { get; set; }

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

        [StringLength(200, ErrorMessage = "O Endereço 1 não pode exceder 200 caracteres.")]
        public string Address1 { get; set; }

        [StringLength(20, ErrorMessage = "O Código Postal não pode exceder 20 caracteres.")]
        public string Address_PostalCode { get; set; }

        [StringLength(100, ErrorMessage = "O País não pode exceder 100 caracteres.")]
        public string Address_Country { get; set; }

        [StringLength(100, ErrorMessage = "O Estado não pode exceder 100 caracteres.")]
        public string Address_State { get; set; }

        [StringLength(100, ErrorMessage = "A Cidade não pode exceder 100 caracteres.")]
        public string Address_City { get; set; }

        [StringLength(200, ErrorMessage = "O Complemento não pode exceder 200 caracteres.")]
        public string Address_Adjunct { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O Tipo de Lead deve ser um valor não negativo.")]
        public int TypeLead { get; set; }

        [Range(0, 99999999999, ErrorMessage = "O CPF deve ser um valor válido.")]
        public int CPF { get; set; }

        [Range(0, 99999999999999, ErrorMessage = "O CNPJ deve ser um valor válido.")]
        public int CNPJ { get; set; }

        // Navigation properties
        public ICollection<OpportunityDTO> Opportunities { get; set; }
    }
}