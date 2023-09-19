namespace Controle.Pedidos.Api.Dtos
{
    public class PedidoFaturadoPublishDto
    {
        public int Id { get; set; }
        public string? NomeCliente { get; set; }
        public double? ValorTotalPedido { get; set; }
        public string? Evento { get; set; }
    }
}
