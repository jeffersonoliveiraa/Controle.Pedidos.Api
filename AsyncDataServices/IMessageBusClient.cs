using Controle.Pedidos.Api.Dtos;

namespace Controle.Pedidos.Api.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishPedidoFaturado(PedidoFaturadoPublishDto pedidoFaturadoPublishDto);
    }
}
