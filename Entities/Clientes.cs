namespace Controle.Pedidos.Api.Entities;

public class Clientes
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }
}
