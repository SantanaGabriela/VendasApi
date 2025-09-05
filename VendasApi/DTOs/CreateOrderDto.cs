namespace VendasApi.DTOs
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderItemCreateDto> Items { get; set; } = new();


    }
}
