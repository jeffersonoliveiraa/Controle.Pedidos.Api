using Controle.Pedidos.Api.Entities;
using Controle.Pedidos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controle.Pedidos.Api.Controllers;

[Route("pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidosService _pedidosService;

    public PedidosController(IPedidosService pedidosService)
    {
        _pedidosService = pedidosService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Pedido>>> GetPedidosAsync()
    {
        try
        {
            var pedidos = await _pedidosService.GetPedidos();
            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter pedidos: {ex.Message}");    
        }
    }

    [HttpGet("{nome}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IAsyncEnumerable<Pedido>>> GetPedidoByNomeAsync(string nome)
    {
        try
        {
            var pedidos = await _pedidosService.GetPedidosByNomeCliente(nome);

            if (pedidos == null)
            {
                return NotFound($"Não existem pedidos no nome do cliente {nome}");
            }

            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter pedido: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Pedido>> GetPedidoByIdAsync(int id)
    {
        try
        {
            var pedido = await _pedidosService.GetPedido(id);

            if (pedido == null)
            {
                return NotFound($"Não existe pedido com o id: {id}");
            }

            return Ok(pedido);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter pedido: {ex.Message}");
        }
    }
}
