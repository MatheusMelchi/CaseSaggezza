using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Entities
{
    public class Fornecedor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public required string Name { get; set; }

        [MaxLength(15)]
        [AllowNull]
        public string? Cellphone { get; set; }


        public ICollection<Entrega> Entregas { get; set; }

    }
}
