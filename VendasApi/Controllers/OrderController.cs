using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using VendasApi.DTOs;
using VendasApi.Models;
using VendasApi.Services;

namespace VendasApi.Controllers
{
    [Authorize]
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

        [HttpGet("api/orders/{orderId}/status")]
        public async Task<ActionResult<GetStatusDto>> GetOrderStatus(int orderId)
        {
            var status = await _orderService.GetOrderStatusAsync(orderId);
            return Ok(status);
        }
        [HttpPost("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateStatus model)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, model);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new {OrderId = orderId,Status = model.Status});
        }

    }
}


