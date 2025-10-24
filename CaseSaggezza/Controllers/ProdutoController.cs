using CaseSaggezza_Dal.Contexts;
using CaseSaggezza_Domain.Dto;
using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseSaggezza.Controllers
{
    [Route("Api/[controller]")]
    public class ProdutoController(CaseSaggezzaDbContext context) : ControllerBase
    {
        private readonly CaseSaggezzaDbContext _context = context;

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var produtos = _context.Produtos.Include(x => x.Entregas).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Entregas = x.Entregas.Select(y => new { id = y.Id, Address = y.Address })
            });

            return Ok(produtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetProduto(int id)
        {
            Produto? produto = _context.Produtos.Include(x => x.Entregas).FirstOrDefault(x => x.Id == id);

            if (produto == null)
                return BadRequest("Fornecedor não encontrado");

            return Ok(
                new 
                { 
                    Id = produto.Id,
                    Name = produto.Name,
                    Entregas = produto.Entregas.Select(y => new { id = y.Id, Address = y.Address }) 
                });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] ProdutoDto produto)
        {
            if (produto == null)
                return BadRequest();

            Produto newProduto = new Produto { Name = produto.Name };

            _context.Produtos.Add(newProduto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProduto), new { id = newProduto.Id }, produto);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest();

            Produto? produtoUpdate = _context.Produtos.Where(x => x.Id == produto.Id).AsNoTracking().FirstOrDefault();

            if (produtoUpdate == null)
                return NotFound();

            _context.Produtos.Update(produto);
            _context.SaveChanges();

            return Ok($"Produto: {produto.Name}, atualizado com sucesso!");
        }

        [HttpDelete("{produtoId}")]
        [Authorize]
        public IActionResult Delete(int produtoId)
        {
            if (produtoId == null)
                return BadRequest();

            Produto? produtoDelete = _context.Produtos.Where(x => x.Id == produtoId).FirstOrDefault();

            if (produtoDelete == null)
                return NotFound();

            _context.Produtos.Remove(produtoDelete);
            _context.SaveChanges();

            return Ok($"Produto: {produtoDelete.Name}, deletada com sucesso!");
        }
    }
}
