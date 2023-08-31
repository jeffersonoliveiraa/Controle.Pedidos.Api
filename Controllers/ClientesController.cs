using Controle.Pedidos.Api.Context;
using Controle.Pedidos.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controle.Pedidos.Api.Controllers;

[Route("clientes")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Clientes>>> GetAllClientesAsync()
    {
        try
        {
            var clientes = await _context.Clientes!.ToListAsync();
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpGet("{nome}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Clientes>>> GetClientesByNomeAsync(string nome)
    {
        try
        {
            List<Clientes> clientes;

            clientes = await _context.Clientes!.Where(n => n.Nome!.Contains(nome)).ToListAsync();

            return Ok(clientes!);
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
    public async Task<ActionResult<Clientes>> CreateCliente([FromBody]Clientes clientes)
    {
        try
        {
            _context.Add(clientes);
            await _context.SaveChangesAsync();

            return Ok(clientes);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpPut("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateCliente(int id, [FromBody] Clientes clientes)
    {
        try
        {
            _context.Entry(clientes).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Cliente com id = {id} foi atualizado com sucesso");
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
    public async Task<IActionResult> DeleteCliente([FromBody] Clientes clientes)
    {
        try
        {
            _context.Clientes!.Remove(clientes);
            await _context.SaveChangesAsync();

            return Ok("Cliente excluido com sucesso");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }
}
