namespace Praksa2.Dtos
{
    public class ProductCreateDto
    { 
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }

}
