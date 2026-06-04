using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Praksa2.Data;
using Praksa2.Models;

namespace Praksa2.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly int _defaultTopCount;


        public ProductRepository(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            var defaultTopCountValue = configuration["DefaultTopCount"];

            if (!int.TryParse(defaultTopCountValue, out _defaultTopCount))
            {
                
                _defaultTopCount = 10; 
            }
        }
        
        public IEnumerable<Products> GetProductsByUserId(int userId)
        {
            return appDbContext.UserProducts
                           .Where(up => up.UserId == userId)
                           .Select(up => up.Product)
                           .ToList();
        }
        public async Task<Products> GetByIdAsync(int id, int userId)
        {
            return await appDbContext.Products
                .Include(p => p.UserProducts)
                 .ThenInclude(up => up.User)
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

        public async Task AssignProductToUser(int userId, int productId)
        {
            var userProduct = new UserProduct
            {
                UserId = userId,
                ProductId = productId
            };
            await appDbContext.UserProducts.AddAsync(userProduct);
            await appDbContext.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(int id, Products product, int userId)
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
        public async Task DeleteAsync(int id, int userId)
        {
            var product = await appDbContext.Products.FindAsync(id);
            if (product != null)
            {
                appDbContext.Products.Remove(product);
                await appDbContext.SaveChangesAsync();
            }
        }
        public async Task<int> GetTotalProductCountAsync()
        {
            return await appDbContext.Products.CountAsync();
        }
        public async Task<int> GetAveragePriceAsync()
        {
            var averagePrice = await appDbContext.Products.AverageAsync(p => p.Price);

            return Convert.ToInt32(averagePrice);

        }
        public async Task<int> GetLowestPriceAsync()
        {
            return await appDbContext.Products.MinAsync(p => p.Price);
        }

        public async Task<int> GetHighestPriceAsync()
        {
            return await appDbContext.Products.MaxAsync(p => p.Price);
        }
        public async Task<int> GetTotalAssignedProductsCountAsync()
        {
            return await appDbContext.UserProducts.CountAsync();
        }
        public async Task<string> GetUsernameByIdAsync(int userId)
        {
            var user = await appDbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Username)
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<List<ProductPopularity>> GetTopPopularProductsAsync(int? topCount = null)
        {
            var count = topCount ?? _defaultTopCount;
            return await appDbContext.UserProducts
                .GroupBy(up => up.Product)
                .Select(g => new ProductPopularity
                {
                    ProductId = g.Key.Id,
                    ProductName = g.Key.Name,
                    TotalAssignments = g.Count(),
                    CreatorName = g.Key.CreatedByUser
                })
                .Where(p => p.TotalAssignments > 0)
                .OrderByDescending(p => p.TotalAssignments)
                .Take(count)
                .ToListAsync();
        }
    }
}
