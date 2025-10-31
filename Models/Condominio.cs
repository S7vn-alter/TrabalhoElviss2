using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabalhoElvis2.Models
{
    public class Condominio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do condomínio é obrigatório.")]
        [StringLength(120)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18, ErrorMessage = "Use o formato 00.000.000/0000-00")]
        public string Cnpj { get; set; }

        [StringLength(200)]
        public string Endereco { get; set; }

        [Range(1, 999, ErrorMessage = "Informe a quantidade de blocos (mín. 1)")]
        public int? QuantidadeBlocos { get; set; }

        [Range(1, 9999, ErrorMessage = "Informe as unidades por bloco (mín. 1)")]
        public int? UnidadesPorBloco { get; set; }

        [Required]
        public int AdminUsuarioId { get; set; }

        [ForeignKey(nameof(AdminUsuarioId))]
        public Usuario Admin { get; set; }
    }
}