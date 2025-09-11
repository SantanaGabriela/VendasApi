using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
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

            foreach (var item in createorderDto.Items)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(
                    $"https://localhost:7177/api/products/{item.ProductId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception($"Produto {item.ProductId} não encontrado no estoque");
                }

                response.EnsureSuccessStatusCode();


                var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();

                if (products == null || products.Count == 0)
                {
                    throw new Exception($"Produto {item.ProductId} não encontrado no estoque");
                }

                var product = products.First();

                if (product.Stock < item.Quantity)
                {
                    throw new Exception(
                        $"Estoque insuficiente para o produto {item.ProductId}. " +
                        $"Disponível: {product.Stock}, solicitado: {item.Quantity}");
                }

            }

            var order = new Order
            {
                CustomerId = createorderDto.CustomerId,
                CustomerName = createorderDto.CustomerName,
                Items = createorderDto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                }).ToList(),
                Status = "pendente"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await PublishOrderCreatedMesageAsync(order);
            return "Pedido criado Com Sucesso!";
        }

        public async Task<GetStatusDto> GetOrderStatusAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Pedido não encontrado");
            }
            return new GetStatusDto { Status = order.Status };
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = status.Status;
            await _context.SaveChangesAsync();
            return true;
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

