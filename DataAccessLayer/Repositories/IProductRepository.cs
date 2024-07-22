using Praksa2.Models;

namespace Praksa2.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products> GetByIdAsync(int id);
        Task AddAsync(Products product);
        Task UpdateAsync(Products product);
        Task DeleteAsync(int id);
    }
}
