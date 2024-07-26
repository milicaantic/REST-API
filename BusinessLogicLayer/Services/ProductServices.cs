using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Praksa2.Data;
using Praksa2.Models;
using Praksa2.Repositories;
using System;
using System.Linq;

namespace Praksa2.Services
{
    public class ProductServices : IProductServices
    {

        private readonly IProductRepository productRepository;

        public ProductServices(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

    
        public IEnumerable<Products> GetUserProducts(int userId)
        {
            return productRepository.GetProductsByUserId(userId);
        }
        public async Task<Products> GetProductById(int id, int userId)
        {
            try
            {
                return await productRepository.GetByIdAsync(id, userId); ;
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
               /* var userProduct = new UserProduct
                {
                    UserId = product.OwnerId,
                    ProductId = product.Id
                };
                await productRepository.AddUserProductAsync(userProduct);*/
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error adding the product", ex);
            }
        }


        public void AssignProductToUser(int userId, int productId)
        {

            productRepository.AssignProductToUser(userId, productId);
        }
        public async Task UpdateProduct(int id, Products newProduct, int userId)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id, userId);
                if (product == null)
                { throw new InvalidOperationException("Product not found."); }
                product.Name = newProduct.Name;
                product.Description = newProduct.Description;
                product.Price = newProduct.Price;
                await productRepository.UpdateAsync(id, product, userId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating the product", ex);
            }

        }
        public async Task DeleteProduct(int id, int userId)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id, userId);
                if (product != null)
                {
                    await productRepository.DeleteAsync(id, userId);

                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error deleting the product", ex);
            }
        }
        public async Task<int> GetTotalProductCountAsync()
        {
            return await productRepository.GetTotalProductCountAsync();
        }
        public async Task<int> GetAveragePriceAsync()
        {
            return await productRepository.GetAveragePriceAsync();
        }
        public async Task<int> GetLowestPriceAsync()
        {
            return await productRepository.GetLowestPriceAsync();
        }

        public async Task<int> GetHighestPriceAsync()
        {
            return await productRepository.GetHighestPriceAsync();
        }
        public async Task<int> GetTotalAssignedProductsCountAsync()
        {
            return await productRepository.GetTotalAssignedProductsCountAsync();
        }
        public async Task<List<ProductPopularity>> GetTopPopularProductsAsync(int? topCount = null)
        {
            return await productRepository.GetTopPopularProductsAsync(topCount);
        }

    }
}
