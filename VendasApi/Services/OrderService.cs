using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VendasApi.Data;
using VendasApi.DTOs;
using VendasApi.Models;

namespace VendasApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;

        public OrderService(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateOrderAsync(CreateOrderDto createorderDto)
        {
            var order = new Order
            {
                CustomerId = createorderDto.CustomerId,
                CustomerName = createorderDto.CustomerName,
                Items = createorderDto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                }).ToList()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await PublishOrderCreatedMesageAsync(order);
            return "Pedido criado Com Sucesso!";
        }
        private async Task PublishOrderCreatedMesageAsync(Order order)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "pedido", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            foreach (var item in order.Items)
            {
                item.Order = null;
            }
            string jsonString = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "pedido", body: body);
            return;
        }


    }
}

