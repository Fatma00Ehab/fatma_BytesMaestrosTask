namespace backend.Models.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }

        public IFormFile? Image { get; set; }
    }
}
