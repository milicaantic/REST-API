using Praksa2.Models;

namespace Praksa2.Services
{
    public interface IProductServices
    {
       Task<IEnumerable<Products>> GetAllProducts();
       Task<Products> GetProductById(int id);
       Task AddProduct(Products product);
        Task UpdateProduct(Products product);
        Task DeleteProduct(int id);
    }
}
