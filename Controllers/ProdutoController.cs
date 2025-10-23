using CaseSaggezza_Dal.Contexts;
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

        [HttpGet("GetProdutos")]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(_context.Produtos.ToList());
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetProduto(int id)
        {
            Produto? produto = _context.Produtos.Find(id);

            if (produto == null)
                return BadRequest("Fornecedor não encontrado");

            return Ok(produto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest();

            Produto? produtoUpdate = _context.Produtos.Where(x => x.Id == produto.Id).AsNoTracking().FirstOrDefault();

            if (produto == null)
                return NotFound();

            _context.Produtos.Update(produto);
            _context.SaveChanges();

            return Ok($"Produto: {produto.Name}, atualizado com sucesso!");
        }
    }
}
