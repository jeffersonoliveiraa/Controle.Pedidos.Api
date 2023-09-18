using Controle.Pedidos.Api.Context;
using Controle.Pedidos.Api.Entities;
using Microsoft.EntityFrameworkCore;

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

            pedido.Faturado = false;

            foreach (var item in pedido.Produtos!)
            {
                if (item.Ativo == false)
                {
                    pedido.Produtos.Remove(item);
                }
            }

            _context.Add(pedido);
            await _context.SaveChangesAsync();

            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            throw;
        }
    }

    public async Task<int> DeletePedido(Pedido pedido)
    {
        try
        {
            _context.Pedidos!.Remove(pedido);
            await _context.SaveChangesAsync();

            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            throw;
        }
    }

    public async Task<Pedido> GetPedido(int id)
    {
        try
        {
            var pedido = await _context.Pedidos!.FindAsync(id);

            return pedido!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Pedido>> GetPedidos()
    {
        try
        {
            return await _context.Pedidos!.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Pedido>> GetPedidosByNomeCliente(string nome)
    {
        IEnumerable<Pedido> pedidos;

        if (!string.IsNullOrWhiteSpace(nome))
        {
            pedidos = await _context.Pedidos!.Where(n => n.Clientes!.Nome!.Contains(nome)).ToListAsync();
        }
        else
        {
            pedidos = await GetPedidos();
        }

        return pedidos;
    }

    public async Task<int> UpdatePedido(Pedido pedido)
    {
        try
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}
