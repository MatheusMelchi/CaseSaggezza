using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Dto
{
    public record LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
