using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Dto;
using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseSaggezza.Controllers
{
    [Route("Api/[controller]")]
    public class EntregaController(CaseSaggezzaDbContext context) : ControllerBase
    {
        private readonly CaseSaggezzaDbContext _context = context;

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var entregas = _context.Entregas.Include(x => x.Produtos).Include(y => y.Fornecedores)
                                               .Select(x => new
                                               {
                                                   Id = x.Id, 
                                                   Address = x.Address,
                                                   Produtos = x.Produtos.Select(y => new { id = y.Id, Name = y.Name}),
                                                   Fornecedores = x.Fornecedores.Select(y => new { id = y.Id, Name = y.Name})
                                               });


            return Ok(entregas);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetEntrega(int id)
        {
            Entrega? entrega = _context.Entregas.Include(x => x.Produtos).Include(y => y.Fornecedores).FirstOrDefault(x => x.Id == id);

            if (entrega == null)
                return BadRequest("Entrega não encontrada");

            return Ok(entrega);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] EntregaDto entrega)
        {
            if (entrega == null)
                return BadRequest();

            Entrega newEntrega = new Entrega { Address = entrega.Address };

            if (entrega.Produtos.Where(x => x.ProdutoId != 0).Any())
                newEntrega.Produtos = _context.Produtos.Where(x => entrega.Produtos.Select(y => y.ProdutoId).Contains(x.Id)).ToList();

            if (entrega.Fornecedores.Where(x => x.FornecedorId != 0).Any())
                newEntrega.Fornecedores = _context.Fornecedores.Where(x => entrega.Fornecedores.Select(y => y.FornecedorId).Contains(x.Id)).ToList();

            _context.Entregas.Add(newEntrega);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEntrega), new { id = newEntrega.Id }, entrega);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Entrega entrega)
        {
            if (entrega == null)
                return BadRequest();

            Entrega? entregaDelete = _context.Entregas.Where(x => x.Id == entrega.Id).AsNoTracking().FirstOrDefault();

            if (entregaDelete == null)
                return NotFound();

            _context.Entregas.Remove(entrega);
            _context.SaveChanges();

            return Ok($"Entrega: {entrega.Address}, deletada com sucesso!");
        }

        [HttpDelete("{entregaId}")]
        [Authorize]
        public IActionResult Delete(int entregaId)
        {
            if (entregaId == null)
                return BadRequest();

            Entrega? entregaDelete = _context.Entregas.Where(x => x.Id == entregaId).FirstOrDefault();

            if (entregaDelete == null)
                return NotFound();

            _context.Entregas.Remove(entregaDelete);
            _context.SaveChanges();

            return Ok($"Produto: {entregaDelete.Address}, deletada com sucesso!");
        }
    }
}
