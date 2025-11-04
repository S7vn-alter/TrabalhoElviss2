using System;
using System.ComponentModel.DataAnnotations;

namespace TrabalhoElvis2.Models
{
    public class Contrato
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Número do Contrato")]
        [Required]
        public string NumeroContrato { get; set; } = string.Empty;

        [Display(Name = "Imóvel")]
        [Required]
        public string Imovel { get; set; } = string.Empty;

        [Display(Name = "Locatário")]
        public string? Locatario { get; set; }

        [Display(Name = "Valor do Aluguel")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        [DataType(DataType.Date)]
        public DateTime DataTermino { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Ativo";
    }
}