using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Dto;
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

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var fornecedores = _context.Fornecedores.Include(x => x.Entregas).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Entregas = x.Entregas.Select(y => new { id = y.Id, Address = y.Address })
            });

            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetFornecedor(int id)
        {
            Fornecedor? fornecedor = _context.Fornecedores.Include(x => x.Entregas).FirstOrDefault(x => x.Id == id);

            if (fornecedor == null)
                return BadRequest("Fornecedor não encontrado");

            return Ok(
                new
                {
                    Id = fornecedor.Id,
                    Name = fornecedor.Name,
                    Entregas = fornecedor.Entregas.Select(y => new { id = y.Id, Address = y.Address })
                });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] FornecedorDto fornecedor)
        {
            if (fornecedor == null)
                return BadRequest();

            Fornecedor newFornecedor = new Fornecedor { Name = fornecedor.Name };

            _context.Fornecedores.Add(newFornecedor);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetFornecedor), new { id = newFornecedor.Id }, fornecedor);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
                return BadRequest();

            Fornecedor? fornecedorUpdate = _context.Fornecedores.Where(x => x.Id == fornecedor.Id).AsNoTracking().FirstOrDefault();

            if (fornecedorUpdate == null)
                return NotFound();

            _context.Fornecedores.Update(fornecedor);
            _context.SaveChanges();

            return Ok($"Fornecedor: {fornecedor.Name}, atualizado com sucesso!");
        }

        [HttpDelete("{fornecedorId}")]
        [Authorize]
        public IActionResult Delete(int fornecedorId)
        {
            if (fornecedorId == null)
                return BadRequest();

            Fornecedor? fornecedorDelete = _context.Fornecedores.Where(x => x.Id == fornecedorId).FirstOrDefault();

            if (fornecedorDelete == null)
                return NotFound();

            _context.Fornecedores.Remove(fornecedorDelete);
            _context.SaveChanges();

            return Ok($"Fornecedor: {fornecedorDelete.Name}, deletado com sucesso!");
        }
    }
}
