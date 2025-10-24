using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Entities
{
    public class EntregaProduto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Entrega")]
        public required int EntregaId { get; set; }
        public virtual Entrega? Entrega { get; set; }
        
        [Required]
        [ForeignKey("Produto")]
        public required int ProdutoId { get; set; }
        public virtual Produto? Produto { get; set; }
    }
}
