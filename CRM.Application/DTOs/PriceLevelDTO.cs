using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class PriceLevelDTO
    {
        public Guid PriceLevelID { get; set; }

        [Required(ErrorMessage = "O campo Nome do Nível é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome do Nível não pode exceder 100 caracteres.")]
        public string LevelName { get; set; }

        [Required(ErrorMessage = "O campo Percentual de Desconto é obrigatório.")]
        [Range(0, 100, ErrorMessage = "O Percentual de Desconto deve estar entre 0 e 100.")]
        public decimal DiscountPercentage { get; set; }

        [Required(ErrorMessage = "O campo Valor Base é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Valor Base deve ser um valor positivo.")]
        public decimal ValueBase { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }
    }
}