using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Domain.Dto
{
    public class RegisterRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Name { get; set; }
    }
}
