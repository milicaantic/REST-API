using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Praksa2.Data;
using Praksa2.Models;
using Praksa2.Repositories;
using System.Linq;

namespace Praksa2.Services
{
    public class ProductServices : IProductServices
    {
        //private readonly List<Products> productList = new List<Products>();
       /* private readonly AppDbContext appDbContext;
        public ProductServices(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
       */
        private readonly IProductRepository productRepository;

        public ProductServices(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            try
            {
                return await productRepository.GetAllAsync();
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error retrieving products", ex);
            }
        }
        public async Task<Products> GetProductById(int id)
        {
            try
            {
                return await productRepository.GetByIdAsync(id); ;
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error retrieving product", ex);
            }
        }
        public async Task AddProduct(Products product)
        {
            try
            {
                await productRepository.AddAsync(product);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error adding the product", ex);
            }
        }
        public async Task UpdateProduct(int id, Products newProduct)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);
                if (product == null)
                { throw new InvalidOperationException("Product not found."); }
                product.Name = newProduct.Name;
                product.Description = newProduct.Description;
                product.Price = newProduct.Price;
                await productRepository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating the product", ex);
            }

        }
        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);
                if (product != null)
                {
                    await productRepository.DeleteAsync(id);

                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error updating the product", ex);
            }
        }

    }
}
