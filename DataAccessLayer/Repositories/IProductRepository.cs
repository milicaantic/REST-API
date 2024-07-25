using DataAccessLayer.Models;
using Praksa2.Models;

namespace Praksa2.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        IEnumerable<Products> GetProductsByUserId(int userId);
        Task<Products> GetByIdAsync(int id,int userId);
        Task AddAsync(Products product);
        Task AddUserProductAsync(UserProduct userProduct);
        Task<int> SaveChangesAsync();
        void AssignProductToUser(int userId, int productId);
        Task<bool> UpdateAsync(int id,Products product,int userId);
        Task DeleteAsync(int id,int userId);
    }
}
