using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseSaggezza.Controllers
{
    [Route("Api/[controller]")]
    public class FornecedorController(CaseSaggezzaDbContext context) : ControllerBase
    {
        private readonly CaseSaggezzaDbContext _context = context;

        [HttpGet("GetFornecedores")]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(_context.Fornecedores.ToList());
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetFornecedor(int id)
        {
            Fornecedor? fornecedor = _context.Fornecedores.Find(id);

            if (fornecedor == null)
                return BadRequest("Fornecedor não encontrado");

            return Ok(fornecedor);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
                return BadRequest();

            _context.Fornecedores.Add(fornecedor);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetFornecedor), new { id = fornecedor.Id }, fornecedor);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
                return BadRequest();

            Fornecedor? fornecedorUpdate = _context.Fornecedores.Where(x => x.Id == fornecedor.Id).AsNoTracking().FirstOrDefault();

            if (fornecedor == null)
                return NotFound();

            _context.Fornecedores.Update(fornecedor);
            _context.SaveChanges();

            return Ok($"Fornecedor: {fornecedor.Name}, atualizado com sucesso!");
        }
    }
}
