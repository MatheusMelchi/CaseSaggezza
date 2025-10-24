using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Dto
{
    public class FornecedorEntregaDTO
    {
        [Required]
        public required int FornecedorId { get; set; }
    }
}
