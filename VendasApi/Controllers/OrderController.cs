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
        
    }
}


