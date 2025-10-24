using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Dto
{
    public class ProdutoDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
