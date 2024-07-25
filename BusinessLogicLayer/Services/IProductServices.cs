using Praksa2.Models;

namespace Praksa2.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<Products>> GetAllProducts();
        IEnumerable<Products> GetUserProducts(int userId);
        Task<Products> GetProductById(int id,int userId);
        Task AddProduct(Products product);
        void AssignProductToUser(int userId, int productId);
        Task UpdateProduct(int id, Products product,int userId);
        Task DeleteProduct(int id,int userId);
    }
}
