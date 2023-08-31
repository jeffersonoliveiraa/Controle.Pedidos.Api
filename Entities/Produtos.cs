namespace Controle.Pedidos.Api.Entities;

public class Produtos
{
    public int Id { get; set; }
    public string? NomeProduto { get; set; }
    public double Valor { get; set; }
    public bool Ativo { get; set; }
}
