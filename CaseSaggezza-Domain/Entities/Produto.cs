using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Entities
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public required string Name { get; set; }


        public ICollection<Entrega> Entregas { get; set; }
    }
}
