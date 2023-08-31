namespace Controle.Pedidos.Api.Entities;

public class Pedido
{
    public int Id { get; set; }
    public Clientes? Clientes { get; set; }
    public List<Produtos>? Produtos { get; set; }
    public double Total { get; set; }
    public bool Faturado { get; set; }
}
