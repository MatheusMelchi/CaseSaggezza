using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Dto
{
    public class EntregaDto
    {
        [Required]
        public required string Address { get; set; }

        [AllowNull]
        public IEnumerable<ProdutoEntregaDTO> Produtos { get; set; }

        [AllowNull]
        public IEnumerable<FornecedorEntregaDTO> Fornecedores { get; set; }
    }

}
