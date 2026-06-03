using DataAccessLayer.Models;
using Praksa2.Models;

namespace Praksa2.Repositories
{
    public interface IProductRepository
    {
  
        IEnumerable<Products> GetProductsByUserId(int userId);
        Task<Products> GetByIdAsync(int id,int userId);
        Task AddAsync(Products product);
        Task AddUserProductAsync(UserProduct userProduct);
        
        Task<int> GetTotalProductCountAsync();
        Task AssignProductToUser(int userId, int productId);
        Task<bool> UpdateAsync(int id,Products product,int userId);
        Task DeleteAsync(int id,int userId);
        Task<int> GetAveragePriceAsync();
        Task<int> GetLowestPriceAsync();
        Task<int> GetHighestPriceAsync();
        Task<int> GetTotalAssignedProductsCountAsync();
        Task<List<ProductPopularity>> GetTopPopularProductsAsync(int? topCount=null);
        Task<string> GetUsernameByIdAsync(int userId);
    }
}
