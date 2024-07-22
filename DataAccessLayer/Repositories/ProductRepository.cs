using Microsoft.EntityFrameworkCore;
using Praksa2.Data;
using Praksa2.Models;

namespace Praksa2.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await appDbContext.Products.ToListAsync();
        }
        public async Task<Products> GetByIdAsync(int id)
        {
            return await appDbContext.Products.FindAsync(id);
        }
        public async Task AddAsync(Products product)
        {
            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Products product)
        {
            appDbContext.Products.Update(product);
            await appDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await appDbContext.Products.FindAsync(id);
            if (product != null)
            {
                appDbContext.Products.Remove(product);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}
