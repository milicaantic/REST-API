using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Praksa2.Data;
using Praksa2.Models;
using System.Linq;

namespace Praksa2.Services
{
    public class ProductServices : IProductServices
    {
        //private readonly List<Products> productList = new List<Products>();
        private readonly AppDbContext appDbContext;
        public ProductServices(AppDbContext appDbContext) 
         { this.appDbContext = appDbContext; }
        public async Task<IEnumerable<Products>> GetAllProducts()
        { return await appDbContext.Products.ToListAsync(); }
        public async Task <Products> GetProductById(int id)
        { 
            return await appDbContext.Products.FindAsync(id);  }
        public async Task AddProduct(Products product)
        { await appDbContext.Products.AddAsync(product);
           await appDbContext.SaveChangesAsync();
        }
        public async Task UpdateProduct(Products newProduct)
        {  
            var product = await appDbContext.Products.FindAsync(newProduct.Id);
            if (product == null)
            { throw new InvalidOperationException("Product not found."); }
            product.Name = newProduct.Name;
            product.Id = newProduct.Id;
            product.Description = newProduct.Description;
            product.Price = newProduct.Price;
            appDbContext.Products.Update(product);
            await appDbContext.SaveChangesAsync();

        }
        public async Task DeleteProduct(int id)
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
