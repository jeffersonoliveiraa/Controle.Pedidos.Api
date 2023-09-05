using Controle.Pedidos.Api.Context;
using Controle.Pedidos.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controle.Pedidos.Api.Controllers;

[Route("produtos")]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Produtos>>> GetAllProdutosAsync()
    {
        try
        {
            var produtos = await _context.Produtos!.ToListAsync();
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpGet("nome")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Produtos>>> GetProdutoByNome([FromQuery] string nome)
    {
        try
        {
            List<Produtos> produtos;

            produtos = await _context.Produtos!.Where(x => x.NomeProduto!.Equals(nome)).ToListAsync();

            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Produtos>> CreateProduto([FromBody] Produtos produtos)
    {
        try
        {
            _context.Add(produtos);
            await _context.SaveChangesAsync();

            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateProdutos(int id, [FromBody] Produtos produtos)
    {
        try
        {
            _context.Entry(produtos).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Produto com o id {id} foi atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProdutos([FromBody] Produtos produtos)
    {
        try
        {
            _context.Produtos!.Remove(produtos);
            await _context.SaveChangesAsync();

            return Ok("Produto excluido com sucesso");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }
}
