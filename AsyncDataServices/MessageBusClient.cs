﻿using Controle.Pedidos.Api.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Controle.Pedidos.Api.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _connection = new ConnectionFactory() {HostName = "localhost", Port = 5672 }.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "faturar", type: ExchangeType.Fanout);
    }
    public void PublishPedidoFaturado(PedidoFaturadoPublishDto pedidoFaturadoPublishDto)
    {
        var message = JsonSerializer.Serialize(pedidoFaturadoPublishDto);
        EnviaMensagem(message);
    }

    private void EnviaMensagem(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "faturar",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
