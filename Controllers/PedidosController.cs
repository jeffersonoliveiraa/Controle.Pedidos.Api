using Controle.Pedidos.Api.AsyncDataServices;
using Controle.Pedidos.Api.Dtos;
using Controle.Pedidos.Api.Entities;
using Controle.Pedidos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controle.Pedidos.Api.Controllers;

[Route("pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidosService _pedidosService;
    private readonly IMessageBusClient _messageBusClient;

    public PedidosController(IPedidosService pedidosService, IMessageBusClient messageBusClient)
    {
        _pedidosService = pedidosService;
        _messageBusClient = messageBusClient;
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Pedido>> CreatePedido([FromBody] Pedido pedido)
    {
        try
        {
            await _pedidosService.CreatePedido(pedido);
            return Ok(pedido);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao criar pedido: {ex.Message}");
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdatePedido([FromBody] Pedido pedido)
    {
        try
        {
            await _pedidosService.UpdatePedido(pedido);
            return Ok(pedido);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar pedido: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePedido(int id)
    {
        try
        {
            var pedido = await _pedidosService.GetPedido(id);

            if (pedido != null)
            {
                await _pedidosService.DeletePedido(pedido);
                return Ok($"Pedido com o id = {id} excluido com sucesso");
            }
            else
            {
                return NotFound($"Pedido com id = {id} não existe");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao excluir pedido: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateFaturarPedido(int id)
    {
        try
        {
            PedidoFaturadoPublishedDto pedidoFaturado = new();

            var pedido = await _pedidosService.GetPedido(id);

            if(pedido != null)
            {
                foreach (var item in pedido.Produtos!)
                {
                    pedidoFaturado.ValorTotalPedido += item.Valor;   
                }
                pedidoFaturado.Evento = "Pedido_Faturamento";
                _messageBusClient.PublishPedidoFaturado(pedidoFaturado);

                pedido.Faturado = true;

                await _pedidosService.UpdatePedido(pedido);

                return Ok($"Pedido com o id = {id} faturado com sucesso");
            }
            else
            {
                return NotFound($"Pedido com id = {id} não existe");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao faturar pedido: {ex.Message}");
        }
    }
}
