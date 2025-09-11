namespace VendasApi.Models
{
    public class ProductResponse
    {
        
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }

    }
}
