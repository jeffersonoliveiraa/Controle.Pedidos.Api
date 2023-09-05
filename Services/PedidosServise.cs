using Controle.Pedidos.Api.Context;
using Controle.Pedidos.Api.Entities;

namespace Controle.Pedidos.Api.Services;

public class PedidosServise : IPedidosService
{
    private readonly AppDbContext _context;

    public PedidosServise(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreatePedido(Pedido pedido)
    {
        try
        {
            if (pedido.Clientes!.Ativo == false)
                return 0;

            foreach (var item in pedido.Produtos!)
            {
                //Criar exclusão de produtos onde o atributo ativo seja igual a false
            }

            _context.Add(pedido);
            await _context.SaveChangesAsync();

            return 1;

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
