using VendasApi.DTOs;
using VendasApi.Models;
using System.Collections.Generic;

namespace VendasApi.Services
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(CreateOrderDto createorderDto);
    }
}
