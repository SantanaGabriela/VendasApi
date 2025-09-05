using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using VendasApi.DTOs;
using VendasApi.Models;
using VendasApi.Services;

namespace VendasApi.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost("api/orders")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto )
        {
            var Order = await _orderService.CreateOrderAsync(createOrderDto);
            return Ok(Order);
        }
        [HttpPost("api/mensage")]
        public async Task<ActionResult> MensageOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "pedido", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            string jsonString = JsonSerializer.Serialize(createOrderDto);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "pedido", body: body);
            return Ok();
        }
    }
}


