using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return "Pedido criado Com Sucesso!";
        }

 
    }
}

