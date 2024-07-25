using DataAccessLayer.Models;
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
        public IEnumerable<Products> GetProductsByUserId(int userId)
        {
            return appDbContext.UserProducts
                           .Where(up => up.UserId == userId)
                           .Select(up => up.Product)
                           .ToList();
        }
        public async Task<Products> GetByIdAsync(int id,int userId)
        {
            return await appDbContext.Products
                 .Where(p => p.Id == id && p.OwnerId == userId)
                 .FirstOrDefaultAsync();
        }
        public async Task AddAsync(Products product)
        {
            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync(); 
          
        }
        public async Task AddUserProductAsync(UserProduct userProduct)
        {
            await appDbContext.UserProducts.AddAsync(userProduct);
            await appDbContext.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await appDbContext.SaveChangesAsync();
        }
        public void AssignProductToUser(int userId, int productId)
        {
            var userProduct = new UserProduct
            {
                UserId = userId,
                ProductId = productId
            };
            appDbContext.UserProducts.Add(userProduct);
            appDbContext.SaveChanges();
        }
        public async Task<bool> UpdateAsync(int id,Products product,int userId)
        {
            
            var existingProduct = await appDbContext.Products
                .Where(p => p.Id == id && p.OwnerId == userId)
                .FirstOrDefaultAsync();

            if (existingProduct == null)
            {
                return false; 
            }

         
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            await appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(int id,int userId)
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
