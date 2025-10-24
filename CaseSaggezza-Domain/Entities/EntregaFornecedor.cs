using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Entities
{
    public class EntregaFornecedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Entrega")]
        public required int EntregaId { get; set; }
        public virtual Entrega? Entrega { get; set; }

        [Required]
        [ForeignKey("Fornecedor")]
        public required int FornecedorId { get; set; }
        public virtual Fornecedor? Fornecedor { get; set; }
    }
}
