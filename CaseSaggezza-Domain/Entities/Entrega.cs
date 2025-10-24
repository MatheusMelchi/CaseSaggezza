using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Entities
{
    public class Entrega
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Address { get; set; }


        public ICollection<Produto> Produtos { get; set; }
        public ICollection<Fornecedor> Fornecedores { get; set; }
    }
}
