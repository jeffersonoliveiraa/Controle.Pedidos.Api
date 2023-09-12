using Controle.Pedidos.Api.Entities;

namespace Controle.Pedidos.Api.Services;

public interface IPedidosService
{
    Task<IEnumerable<Pedido>> GetPedidos();
    Task<Pedido> GetPedido(int id);
    Task<IEnumerable<Pedido>> GetPedidosByNomeCliente(string nome);
    Task<int> CreatePedido(Pedido pedido);
    Task<int> UpdatePedido(Pedido pedido);
    Task<int> DeletePedido(Pedido pedido);
}
