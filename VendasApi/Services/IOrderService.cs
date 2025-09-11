using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VendasApi.DTOs;
using VendasApi.Models;

namespace VendasApi.Services
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(CreateOrderDto createorderDto);
        Task<GetStatusDto> GetOrderStatusAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, UpdateStatus status);
    }
}
