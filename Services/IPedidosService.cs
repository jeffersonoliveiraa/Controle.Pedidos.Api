using Controle.Pedidos.Api.Entities;

namespace Controle.Pedidos.Api.Services;

public interface IPedidosService
{
    Task<int> CreatePedido(Pedido pedido);
}
