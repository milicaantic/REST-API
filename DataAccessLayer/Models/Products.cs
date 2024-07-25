using DataAccessLayer.Models;

namespace Praksa2.Models
{
    public class Products
    {
        public int Id { get;set; }
        public string? Name {  get; set; }
        public string? Description {  get; set; }
        public decimal Price { get; set; }
        public int OwnerId { get; set; }
       
        public ICollection<UserProduct> UserProducts { get; set; }

    }
}
